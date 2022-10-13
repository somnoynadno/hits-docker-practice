using Mockups.Models.Account;
using Mockups.Storage;

namespace Mockups.Services.Addresses
{
    public interface IAddressesService
    {
        Task AddAddress(AddAddressViewModel model, Guid userId);
        Task EditAddress(EditAddressViewModel model);
        Task DeleteAddress(Guid addressId);
        Task<EditAddressViewModel> GetEditAddressViewModel(Guid addressId);
        Task<AddressShortViewModel> GetAddressShortViewModel(Guid AddressId);
        List<Address> GetAddressesByUserId(Guid userId);
    }
}
