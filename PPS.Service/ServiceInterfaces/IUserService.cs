using PPS.API.Shared.ViewModel;
using PPS.API.Shared.ViewModel.User;
using PPS.Data.Edmx;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface IUserService
    {
        //User GetUserById(int userId);
        Task<User> GetUserByEmailId(string email);
        Task<User> VerifyUserLogOn(string email, string password);
        User AddUser(User user);
        Task<User> GetUserByAspNetUserId(string aspNetUserId);
        User UpdateUser(User user);
        UserPasswordChangeModel UpdatePassword(UserPasswordChangeModel user);
        bool DeleteUserById(int id);
        bool DeleteUserByEmail(string email);
        void SetUserLoginInvalidCount(string email);
        void ResetUserLoginInvalidCount(string email);
        void UnlockUser(string email);

        List<UserVm> GetUsers(UserVm currentUser);
        bool UserLock(int userId);
        bool UserUnock(int userId);
        bool UserActivate(int userId);
        bool UserDeactivate(int userId);
        bool Register(UserRegisterVm user);
        Task<UserVm> GetUserRoleById(int userId);
        Task<bool> UpdateUser(UserVm userVm);
    }
}