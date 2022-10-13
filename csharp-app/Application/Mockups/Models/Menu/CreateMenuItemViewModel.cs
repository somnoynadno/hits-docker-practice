using Mockups.Storage;
using System.ComponentModel.DataAnnotations;

namespace Mockups.Models.Menu
{
    public class CreateMenuItemViewModel
    {
        [Required(ErrorMessage = "Необходимо заполнить название блюда")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить цену блюда")]
        [Range(0, float.MaxValue, ErrorMessage = "Недопустимое значение цены блюда")]
        [Display(Name = "Цена")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Необходимо заполнить описание блюда")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Категория")]
        public MenuItemCategory Category { get; set; }
        [Display(Name = "Блюдо является вегетарианским?")]
        public bool IsVegan { get; set; }
        public IFormFile File { get; set; }
    }
}
