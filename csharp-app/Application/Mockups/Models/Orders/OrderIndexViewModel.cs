namespace Mockups.Models.Orders
{
    public class OrderIndexViewModel
    {
        public List<OrderShortViewModel> Orders { get; set; } = new List<OrderShortViewModel>();
        public bool CartIsEmpty { get; set; }
    }
}
