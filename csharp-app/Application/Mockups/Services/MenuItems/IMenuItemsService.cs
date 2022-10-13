using Mockups.Models.Menu;
using Mockups.Storage;

namespace Mockups.Services.MenuItems
{
    public interface IMenuItemsService
    {
        Task<List<MenuItemViewModel>> GetAllMenuItems(bool? isVegan, MenuItemCategory[]? category);
        Task CreateMenuItem(CreateMenuItemViewModel model);
        Task<bool?> DeleteMenuItem(string id);
        Task<MenuItemViewModel?> GetItemModelById(string id);
        Task AddItemToCart(Guid userID, string itemId, int amount);
        Task<AddToCartViewModel> GetAddToCartModel(string itemId);
        Task<string?> GetItemNameById(Guid itemId);
    }
}
