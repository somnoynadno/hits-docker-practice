using Mockups.Storage;

namespace Mockups.Models.OrdersManagement
{
    public class OrderEditPostViewModel
    {
        public int orderId { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus Status { get; set; }
    }
}
