using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Mockups.Storage
{
    public enum MenuItemCategory
    {
        [Display(Name = "Вок")]
        WOK,
        [Display(Name = "Пицца")]
        Pizza,
        [Display(Name = "Суп")]
        Soup,
        [Display(Name = "Десерт")]
        Dessert,
        [Display(Name = "Напиток")]
        Drink
    }

    public static class MenuItemCategoryHelper
    {
        public static string GetDisplayName(this MenuItemCategory enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                   .First()
                   .GetCustomAttribute<DisplayAttribute>()
                   .Name;
        }
    }
}
