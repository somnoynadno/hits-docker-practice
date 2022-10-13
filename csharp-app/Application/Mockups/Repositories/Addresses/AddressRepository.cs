using Mockups.Storage;
using Microsoft.EntityFrameworkCore;

namespace Mockups.Repositories.Addresses
{
    public class AddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ResetMainAddresses(Guid userId)
        {
            var userAddresses = _context.Addresses.Where(a => a.UserId == userId).ToList();

            foreach (var item in userAddresses)
            {
                item.IsMainAddress = false;
                _context.Entry(item).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddAddress(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task EditAddress(Address address)
        {
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddress(Address address)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public List<Address> GetAddressesByUserId(string userId)
        {
            var guid = new Guid(userId);
            return _context.Addresses.Where(a => a.UserId == guid).ToList();
        }

        public async Task<Address?> GetAddressById(Guid addressId)
        {
            return await _context.Addresses.Where(a => a.Id == addressId).FirstOrDefaultAsync();
        }

        public async Task<bool> UserHasMainAddressSet(Guid userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId && a.IsMainAddress).AnyAsync();
        }

        public async Task<bool> UserHasAnyAddresses(Guid userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId).AnyAsync();
        }

        public async Task SetFirstAddressAsMainForUser (Guid userId)
        {
            (await _context.Addresses.Where(a => a.UserId == userId).FirstAsync()).IsMainAddress = true;
            await _context.SaveChangesAsync();
        }
    }
}
