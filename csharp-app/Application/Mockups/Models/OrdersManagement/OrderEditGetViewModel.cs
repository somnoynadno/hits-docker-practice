using Mockups.Storage;

namespace Mockups.Models.OrdersManagement
{
    public class OrderEditGetViewModel
    {
        public int Id { get; set; }
        public string CreationTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public OrderStatus CurrentStatus { get; set; }
        public OrderStatus? NextStatus { get; set; }
    }
}
