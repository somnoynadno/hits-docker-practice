using Mockups.Models.Cart;

namespace Mockups.Models.OrdersManagement
{
    public class OrderInfoViewModel
    {
        public int Id { get; set; }
        public string CreationTime { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public float TotalCost { get; set; }
        public string Address { get; set; }
        public List<CartMenuItemViewModel> Items { get; set; } = new List<CartMenuItemViewModel>();
    }
}
