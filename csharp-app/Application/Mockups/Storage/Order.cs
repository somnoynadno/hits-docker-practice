namespace Mockups.Storage
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DeliveryTime { get; set; }
        public float Cost { get; set; }
        public float Discount { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public List<OrderMenuItem> OrderMenuItems { get; set; } = new List<OrderMenuItem>();
    }
}
