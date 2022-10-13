using Mockups.Models.Account;
using Mockups.Repositories.Addresses;
using Mockups.Services.Addresses;
using Mockups.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mockups.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAddressesService _addressesService;

        public UsersService(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IAddressesService addressesService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _addressesService = addressesService;
        }


        public async Task Login(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email); // пытаемся найти юзера по email
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email = {model.Email} does not exist.");
            }

            //Проверяем правильность введенного пароля для данного юзера
            var isValidPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isValidPassword)
            {
                throw new InvalidDataException("Incorrect password.");
            }

            // Далее генерируем набор клеймов, состоящих из необходимых для быстрого доступа данных
            var claims = new List<Claim>
            {
                new ("Name", user.Name),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Также в клеймы добавляем все роли пользователя, если они есть
            if (user.Roles?.Any() == true)
            {
                var roles = user.Roles.Select(x => x.Role).ToList();
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            }

            // Задаем параметры аутентификации
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2), // Куки будет жить 2 дня
                IsPersistent = true
            };

            // Процесс авторизации и создания куки
            await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);
        }

        public async Task Register(RegisterViewModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name,
                Phone = model.Phone,
                BirthDate = model.BirthDate
            };

            var userCreationResult = await _userManager.CreateAsync(user, model.Password);


            if (userCreationResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, ApplicationRoleNames.User);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return;
            }
            var errors = string.Join(", ", userCreationResult.Errors.Select(x => x.Description));
            throw new ArgumentException(errors);
        }

        public async Task Logout()
        {
            // Выход из системы == удаление куки
            await _signInManager.SignOutAsync();
        }

        public async Task<EditUserDataViewModel> GetEditUserDataViewModel(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return new EditUserDataViewModel
            {
                Name = user.Name,
                Phone = user.Phone,
                BirthDate = user.BirthDate
            };

        }

        public async Task EditUserData(EditUserDataViewModel model, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            user.Name = model.Name;
            user.Phone = model.Phone;
            user.BirthDate = model.BirthDate;

            await _userManager.UpdateAsync(user);
        }

        public async Task<IndexViewModel> GetUserInfo(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            List<AddressShortViewModel> userAddressModels = new List<AddressShortViewModel>();
            var userAddresses = _addressesService.GetAddressesByUserId(userId);
            foreach (var address in userAddresses)
            {
                userAddressModels.Add(new AddressShortViewModel
                {
                    AddressString = address.GetAddressString(),
                    Note = address.Note,
                    Name = address.Name,
                    Id = address.Id
                });
            }

            return new IndexViewModel
            {
                Phone = user.Phone,
                Name = user.Name,
                BirthDate = user.BirthDate,
                Addresses = userAddressModels
            };
        }
    }
}
