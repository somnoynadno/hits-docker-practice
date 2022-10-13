using Mockups.Models.Cart;
using Mockups.Models.Menu;

namespace Mockups.Services.Carts
{
    public interface ICartsService
    {
        Task AddItemToCart(Guid userId, string itemId, int amount);
        void DeleteItemFromCart(Guid userId, Guid itemId);
        Task<CartIndexViewModel> GetUsersCart(Guid userId, bool isForOrder = false);
        int GetCartItemCount(Guid userId);
        void ClearUsersCart(Guid userId);
    }
}
