using Microsoft.AspNetCore.Identity;

namespace Mockups.Storage
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

}
