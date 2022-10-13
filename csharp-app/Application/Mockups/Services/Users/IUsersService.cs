using Mockups.Models.Account;
using System.Security.Claims;

namespace Mockups.Services.Users
{
    public interface IUsersService
    {
        Task Register(RegisterViewModel model);
        Task Login(LoginViewModel model);
        Task Logout();
        Task<IndexViewModel> GetUserInfo(Guid userId);
        Task EditUserData(EditUserDataViewModel model, Guid userId);
        Task<EditUserDataViewModel> GetEditUserDataViewModel(Guid userId);

    }
}
