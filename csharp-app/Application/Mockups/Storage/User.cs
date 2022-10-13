using Microsoft.AspNetCore.Identity;

namespace Mockups.Storage
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
