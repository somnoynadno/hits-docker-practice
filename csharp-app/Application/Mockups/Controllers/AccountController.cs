using Mockups.Models.Account;
using Mockups.Services.Addresses;
using Mockups.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Mockups.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAddressesService _addressesService;

        public AccountController(IUsersService usersService, IAddressesService addressesService)
        {
            _usersService = usersService;
            _addressesService = addressesService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usersService.Register(model);
                    return RedirectToAction("Index", "Menu");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("RegistrationErrors", ex.Message);
                }
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _usersService.Login(model);
                    return RedirectToAction("Index", "Menu");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Errors", ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _usersService.Logout();
            return RedirectToAction("Index", "Menu");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var userInfo = await _usersService.GetUserInfo(userId);
            return View(userInfo);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    await _addressesService.AddAddress(model, userId);
                    return RedirectToAction("Index", "Account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Errors", ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditAddress(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var addressGuid = Guid.Parse(id);
                var model = await _addressesService.GetEditAddressViewModel(addressGuid);
                return View(model);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditAddress(EditAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _addressesService.EditAddress(model);
                    return RedirectToAction("Index", "Account");
                }
                catch (KeyNotFoundException e)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Errors", ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteAddress(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var addressGuid = Guid.Parse(id);
                var model = await _addressesService.GetAddressShortViewModel(addressGuid);
                return View(model);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [ActionName("DeleteAddress")]
        public async Task<IActionResult> DeleteAddressConfirmed(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var addressGuid = Guid.Parse(id);
                await _addressesService.DeleteAddress(addressGuid);
                return RedirectToAction("Index", "Account");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var model = await _usersService.GetEditUserDataViewModel(userId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    await _usersService.EditUserData(model, userId);
                    return RedirectToAction("Index", "Account");
                }
                catch (KeyNotFoundException e)
                {
                    return NotFound();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Errors", ex.Message);
                }
            }
            return View(model);
        }
    }
}
