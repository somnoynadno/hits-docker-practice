using System.ComponentModel.DataAnnotations;

namespace Mockups.Storage
{
    public class CartMenuItem
    {
        public Guid MenuItemId { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Некорректное число блюд")]
        public int Amount { get; set; } = 1;
    }
}
