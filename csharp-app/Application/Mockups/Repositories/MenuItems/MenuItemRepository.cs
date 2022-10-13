using Mockups.Models.Menu;
using Mockups.Storage;
using Microsoft.EntityFrameworkCore;

namespace Mockups.Repositories.MenuItems
{
    public class MenuItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetAllMenuItems()
        {
            return await _context.MenuItems.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<List<MenuItem>> GetAllMenuItems(MenuItemCategory[] category)
        {
            return await _context.MenuItems.Where(x => x.IsDeleted == false && category.Contains(x.Category)).ToListAsync();
        }

        public async Task<List<MenuItem>> GetAllMenuItems(bool isVegan, MenuItemCategory[] category)
        {
            return await _context.MenuItems
                .Where(x => x.IsDeleted == false
                       && x.IsVegan == isVegan 
                       && category.Contains(x.Category))
                .ToListAsync();
        }

        public async Task<List<MenuItem>> GetAllMenuItems(bool isVegan)
        {
            return await _context.MenuItems.Where(x => x.IsDeleted == false && x.IsVegan == isVegan).ToListAsync();
        }

        public async Task<MenuItem?> GetItemByName(string name)
        {
            return await _context.MenuItems.Where(x => x.IsDeleted == false && x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<MenuItem?> GetItemById(Guid id)
        {
            return await _context.MenuItems.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddItem(MenuItem item)
        {
            await _context.MenuItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(MenuItem item)
        {
            item.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
