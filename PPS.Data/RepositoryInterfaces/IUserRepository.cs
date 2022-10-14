using PPS.Data.Edmx;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel;
using PPS.Data.Dtos.User;
using System.Collections.Generic;
using PPS.API.Shared.ViewModel.User;
using System.Linq;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByEmailId(string email);
        Task<User> VerifyUserLogOn(string email, string password);
        User AddUser(User user);
        Task<User> GetUserByAspNetUserId(string aspNetUserId);
        Task<User> UpdateUser(User user);
        bool DeleteUserById(int id);
        bool DeleteUserByEmail(string email);
        void SetUserLoginInvalidCount(string email);
        void ResetUserLoginInvalidCount(string email);
        void UnlockUser(string email);
        UserPasswordChangeModel UpdatePassword(UserPasswordChangeModel user);

        List<UserDto> GetUsers(UserVm currentUser);
        bool UserLock(int userId);
        bool UserUnlock(int userId);
        bool UserActivate(int userId);
        bool UserDeactivate(int userId);
        bool Register(UserRegisterVm user);
        Task<UserVm> GetUserRoleById(int userId);
        Task<bool> UpdateUser(UserVm userVm);

        IQueryable<User> GetUsers();
    }
}