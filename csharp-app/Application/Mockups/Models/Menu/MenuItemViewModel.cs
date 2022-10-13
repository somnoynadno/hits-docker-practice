using Mockups.Storage;

namespace Mockups.Models.Menu
{
    public class MenuItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public MenuItemCategory Category { get; set; }
        public bool IsVegan { get; set; }
        public string PhotoPath { get; set; }
    }
}
