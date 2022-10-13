namespace Mockups.Storage
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public List<CartMenuItem> Items { get; set; } = new List<CartMenuItem>();
        public DateTime LastUpdated { get; set; }
    }
}
