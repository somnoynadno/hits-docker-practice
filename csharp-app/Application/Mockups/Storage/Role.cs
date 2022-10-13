using Microsoft.AspNetCore.Identity;

namespace Mockups.Storage
{
    public class Role : IdentityRole<Guid>
    {
        public RoleType Type { get; set; }
        public ICollection<UserRole> Users { get; set; }
    }

}
