using System.ComponentModel.DataAnnotations;

namespace Mockups.Models.Account
{
    public class AddAddressViewModel
    {
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        public string? EntranceNumber { get; set; }
        [Required]
        public string FlatNumber { get; set; }
        public string? Note { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsMainAddress { get; set; }
    }
}
