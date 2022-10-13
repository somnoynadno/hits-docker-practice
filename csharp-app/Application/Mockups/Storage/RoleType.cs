using System.ComponentModel.DataAnnotations;

namespace Mockups.Storage
{
    public enum RoleType
    {
        [Display(Name = ApplicationRoleNames.Administrator)]
        Administrator,
        [Display(Name = ApplicationRoleNames.User)]
        User
    }

}
