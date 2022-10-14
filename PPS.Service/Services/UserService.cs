using System;
using PPS.Service.ServiceInterfaces;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using System.Threading.Tasks;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel;
using PPS.API.Shared.ViewModel.User;
using System.Collections.Generic;

namespace PPS.Service.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IRoleService _roleService;
        public UserService()
        {
            _userRepository = new UserRepository();
            _roleService = new RoleService();
        }
        public User AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }
        public async Task<User> GetUserByAspNetUserId(string aspNetUserId)
        {
            return await _userRepository.GetUserByAspNetUserId(aspNetUserId);
        }
        public void SetUserLoginInvalidCount(string email)
        {
            _userRepository.SetUserLoginInvalidCount(email);
        }
        public void ResetUserLoginInvalidCount(string email)
        {
            _userRepository.ResetUserLoginInvalidCount(email);
        }
        public void UnlockUser(string email)
        {
            _userRepository.UnlockUser(email);
        }
        public bool DeleteUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmailId(string email)
        {
            return await _userRepository.GetUserByEmailId(email);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public User UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> VerifyUserLogOn(string email, string password)
        {
            return await _userRepository.VerifyUserLogOn(email, password);
        }

        public UserPasswordChangeModel UpdatePassword(UserPasswordChangeModel user)
        {
            return _userRepository.UpdatePassword(user);
        }

        public List<UserVm> GetUsers(UserVm currentUser)
        {
            var users = _userRepository.GetUsers(currentUser);
            var userVmList = new List<UserVm>();
            if(users.Count > 0)
            {
                users.ForEach(u => {
                    userVmList.Add(u.ToUserVm());
                });                
            }
            return userVmList;
        }

        public bool UserLock(int userId)
        {
            return _userRepository.UserLock(userId);
        }

        public bool UserUnock(int userId)
        {
            return _userRepository.UserUnlock(userId);
        }

        public bool UserActivate(int userId)
        {
            return _userRepository.UserActivate(userId);
        }

        public bool UserDeactivate(int userId)
        {
            return _userRepository.UserDeactivate(userId);
        }

        public bool Register(UserRegisterVm user)
        {
            return _userRepository.Register(user);
        }

        public async Task<UserVm> GetUserRoleById(int userId)
        {
            var roles = await _roleService.GetRoleByUserId(userId);
            var userVm = await _userRepository.GetUserRoleById(userId);
            userVm.Roles = roles;
            return userVm;
        }

        public async Task<bool> UpdateUser(UserVm userVm)
        {
            return await _userRepository.UpdateUser(userVm);
        }
    }
}