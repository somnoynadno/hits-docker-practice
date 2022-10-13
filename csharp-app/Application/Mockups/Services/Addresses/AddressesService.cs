using Mockups.Models.Account;
using Mockups.Repositories.Addresses;
using Mockups.Storage;
using Microsoft.AspNetCore.Identity;

namespace Mockups.Services.Addresses
{
    public class AddressesService : IAddressesService
    {
        private readonly UserManager<User> _userManager;
        private readonly AddressRepository _addressRepository;

        public AddressesService(UserManager<User> userManager,
                AddressRepository addressRepository)
        {
            _userManager = userManager;
            _addressRepository = addressRepository;
        }

        public async Task AddAddress(AddAddressViewModel model, Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            bool isMain = model.IsMainAddress;

            if (!model.IsMainAddress && !_addressRepository.GetAddressesByUserId(userId.ToString()).Any())
            {
                isMain = true;
            }

            var address = new Address
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Note = model.Note,
                StreetName = model.StreetName,
                HouseNumber = model.HouseNumber,
                EntranceNumber = model.EntranceNumber,
                FlatNumber = model.FlatNumber,
                IsMainAddress = isMain,
                UserId = user.Id
            };

            if (address.IsMainAddress && (await _addressRepository.UserHasMainAddressSet(user.Id)))
            {
                await _addressRepository.ResetMainAddresses(user.Id);
            }

            await _addressRepository.AddAddress(address);
        }

        public async Task EditAddress(EditAddressViewModel model)
        {
            var address = await _addressRepository.GetAddressById(model.Id);
            if (address == null)
            {
                throw new KeyNotFoundException("Address not found.");
            }

            if (model.IsMainAddress && !address.IsMainAddress)
            {
                await _addressRepository.ResetMainAddresses(address.UserId);
            }

            address.Name = model.Name;
            address.Note = model.Note;
            address.StreetName = model.StreetName;
            address.HouseNumber = model.HouseNumber;
            address.EntranceNumber = model.EntranceNumber;
            address.FlatNumber = model.FlatNumber;
            address.IsMainAddress = model.IsMainAddress;

            await _addressRepository.EditAddress(address);
        }

        public async Task DeleteAddress(Guid addressId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address == null)
            {
                throw new KeyNotFoundException("Address not found.");
            }
            bool isMain = address.IsMainAddress;
            Guid UserId = address.UserId;

            await _addressRepository.DeleteAddress(address);

            if (isMain && (await _addressRepository.UserHasAnyAddresses(UserId)))
            {
                await _addressRepository.SetFirstAddressAsMainForUser(address.UserId);
            }
        }

        public async Task<AddressShortViewModel> GetAddressShortViewModel(Guid AddressId)
        {
            var address = await _addressRepository.GetAddressById(AddressId);

            if (address == null)
            {
                throw new KeyNotFoundException("Address not found.");
            }

            return new AddressShortViewModel
            {
                Id = address.Id,
                Name = address.Name,
                AddressString = address.GetAddressString(),
                Note = address.Note
            };

        }

        public async Task<EditAddressViewModel> GetEditAddressViewModel(Guid addressId)
        {
            var address = await _addressRepository.GetAddressById(addressId);

            if (address == null)
            {
                throw new KeyNotFoundException("Address not found.");
            }

            return new EditAddressViewModel
            {
                Id = address.Id,
                Name = address.Name,
                Note = address.Note,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                EntranceNumber = address.EntranceNumber,
                FlatNumber = address.FlatNumber,
                IsMainAddress = address.IsMainAddress,
            };
        }

        public List<Address> GetAddressesByUserId(Guid userId)
        {
            return _addressRepository.GetAddressesByUserId(userId.ToString());
        }
    }
}
