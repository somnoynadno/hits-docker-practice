using Mockups.Storage;

namespace Mockups.Repositories.Carts
{
    public class CartsRepository
    {
        private List<Cart> _carts = new List<Cart>();

        public Cart GetUsersCart(Guid userId)
        {
            lock(_carts)
            {
                var cart = _carts.FirstOrDefault(c => c.UserId == userId);
                if (cart == null)
                {
                    var newCart = new Cart
                    {
                        UserId = userId,
                        LastUpdated = DateTime.Now
                    };
                    _carts.Add(newCart);

                    return newCart;
                }

                return cart;
            }
        }

        public void UpdateCart(Guid userId)
        {
            GetUsersCart(userId).LastUpdated = DateTime.Now.AddMinutes(5);
        }

        public void ClearUsersCart(Guid userId)
        {
            GetUsersCart(userId).Items.Clear();
        }

        public void ClearCarts(List<Cart> carts)
        {
            lock(_carts)
            {
                foreach (var cart in carts)
                {
                    _carts.Remove(cart);
                }
            }
        }

        public List<Cart> GetInactiveCarts(int inactiveTime)//в минутах
        {
            lock(_carts)
            {
                return _carts.Where(x => (DateTime.Now - x.LastUpdated).TotalMinutes >= inactiveTime).ToList();
            }
        }

        public void AddItemToCart(Guid userId, CartMenuItem item)
        {
            var cart = GetUsersCart(userId);

            var itemInCart = cart.Items.Where(x => x.MenuItemId == item.MenuItemId).FirstOrDefault();

            if (itemInCart == null)
            {
                lock (_carts)
                {
                    cart.Items.Add(item);
                }
            }
            else
            {
                itemInCart.Amount += item.Amount;
            }

            cart.LastUpdated = DateTime.Now;
        }

        public void DeleteItemFromCart(Guid userId, Guid itemId)
        {
            var cart = GetUsersCart(userId);

            var itemInCart = cart.Items.Where(x => x.MenuItemId == itemId).FirstOrDefault();

            if (itemInCart == null)
                return;

            lock (_carts)
            {
                cart.Items.Remove(itemInCart);
            }

            cart.LastUpdated = DateTime.Now;
        }

        public int GetCartItemCount(Guid userId)
        {
            return GetUsersCart(userId).Items.Count;
        }
    }
}
