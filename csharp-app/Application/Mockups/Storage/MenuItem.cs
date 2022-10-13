using System.ComponentModel.DataAnnotations;

namespace Mockups.Storage
{
    public class MenuItem
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public MenuItemCategory Category { get; set; }
        public bool IsVegan { get; set; }
        public string PhotoPath { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<OrderMenuItem> OrderMenuItems { get; set; } = new List<OrderMenuItem>();
    }
}
