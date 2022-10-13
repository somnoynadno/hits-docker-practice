namespace Mockups.Models.Account
{
    public class IndexViewModel
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public List<AddressShortViewModel> Addresses { get; set; }
    }
}
