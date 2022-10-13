using Mockups.Storage;

namespace Mockups.Models.Menu
{
    public class MenuIndexViewModel
    {
        public List<MenuItemViewModel> Items { get; set; } = new List<MenuItemViewModel>();
        public int ItemsInCart { get; set; } = 0;
    }
}
