using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mockups.Storage
{
    public class OrderMenuItem
    {
        public int OrderId { get; set; }
        public Guid ItemId { get; set; }

        public Order Order { get; set; }
        public MenuItem Item { get; set; }


        public int Amount { get; set; }
    }
}
