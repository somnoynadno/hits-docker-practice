using Mockups.Models.Cart;
using System.ComponentModel.DataAnnotations;

namespace Mockups.Models.Menu
{
    public class AddToCartViewModel
    {
        [Required]
        public MenuItemViewModel Item { get; set; }
        [Required]
        [Display(Name = "Количество")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Некорректное значение числа блюд")]
        public int Amount { get; set; } = 1;
    }
}
