using Mockups.Models.Account;
using Mockups.Models.Cart;
using Mockups.Models.Menu;

namespace Mockups.Models.Orders
{
    public class OrderCreateGetViewModel
    {
        public List<CartMenuItemViewModel> Items { get; set; } = new List<CartMenuItemViewModel>();
        public List<string> Addresses { get; set; } = new List<string>();
        public List<DateTime> DeliveryTimeOptions { get; set; } = new List<DateTime>();
        public float Price { get; set; }
        public float Discount { get; set; }
        public string DiscountDescription { get; set; } = "";
    }
}
