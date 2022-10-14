using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using PPS.Data.RepositoryInterfaces;
using System.Threading.Tasks;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel;
using PPS.Core;
using PPS.API.Shared.ViewModel.User;
using PPS.API.Shared.Extensions;
using PPS.Data.Dtos.User;
using PPS.Data.Enums;
using System.Collections.Concurrent;

namespace PPS.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private PPSDbContext _ppsDbContext;
        private IRoleRepository _roleRepository;
        public UserRepository()
        {
            _ppsDbContext = new PPSDbContext();
            _roleRepository = new RoleRepository();
        }
        public User AddUser(User user)
        {
            return _ppsDbContext.User.Add(user);
        }
        public async Task<User> GetUserByAspNetUserId(string aspNetUserId)
        {
            var user = await _ppsDbContext.User.FirstOrDefaultAsync(x => x.AspNetUserId == aspNetUserId);
            return user;
        }
        public void SetUserLoginInvalidCount(string email)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Email == email);
            var company = _ppsDbContext.Company.FirstOrDefault(x => x.Id == user.CompanyId);
            user.InvalidCount += 1;
            if (user.InvalidCount >= company.AllowedInvalid)
            {
                user.Locked = true;
            }
            _ppsDbContext.User.Attach(user);
            _ppsDbContext.Entry(user).State = EntityState.Modified;
            _ppsDbContext.SaveChanges();
        }
        public void ResetUserLoginInvalidCount(string email)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Email == email);
            user.InvalidCount = 0;
            _ppsDbContext.User.Attach(user);
            _ppsDbContext.Entry(user).State = EntityState.Modified;
            _ppsDbContext.SaveChanges();
        }
        public void UnlockUser(string email)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Email == email);
            user.InvalidCount = 0;
            user.Locked = false;
            _ppsDbContext.User.Attach(user);
            _ppsDbContext.Entry(user).State = EntityState.Modified;
            _ppsDbContext.SaveChanges();
        }
        public bool DeleteUserByEmail(string email)
        {
            var user = _ppsDbContext.User.Include(x => x.UserRole).FirstOrDefault(u => u.Email == email);
            var removedUser = _ppsDbContext.User.Remove(user);
            if (removedUser != null)
                return true;
            return false;
        }

        public bool DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmailId(string email)
        {
            return await _ppsDbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> VerifyUserLogOn(string email, string password)
        {
            try
            {
                var user = await _ppsDbContext.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

                if (user == null)
                    return null;
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserPasswordChangeModel UpdatePassword(UserPasswordChangeModel userModel)
        {
            try
            {
                var user = _ppsDbContext.User.FirstOrDefault(u => u.Email == userModel.Email);
                if (user == null)
                {
                    throw new Exception("Invalid email address.");
                }
                var decryptedPassword = AESThenHMAC.DecryptWithPassword(user.Password, user.PasswordKey);
                if (userModel.CurrentPassword != decryptedPassword)
                {
                    throw new Exception("Invalid password.");
                }
                var encryptedPassword = AESThenHMAC.EncryptWithPassword(userModel.NewPassword, user.PasswordKey);
                user.Password = encryptedPassword;
                _ppsDbContext.User.Attach(user);
                _ppsDbContext.Entry(user).State = EntityState.Modified;
                _ppsDbContext.SaveChanges();

                return userModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserDto> GetUsers(UserVm currentUser)
        {
            var users = _ppsDbContext.User.Where(x => x.CompanyId == currentUser.CompanyId).ToList();
            var userVmList = new List<UserDto>();
            if (users.Count > 0)
            {
                users.ForEach(u =>
                {
                    userVmList.Add(new UserDto
                    {
                        Id = u.Id,
                        AspNetUserId = u.AspNetUserId,
                        Email = u.Email,
                        Name = StringExtension.ToFullName(u.FirstName, u.LastName),
                        Locked = u.Locked == true ? "Locked" : "",
                        Status = u.UserStatus.Status
                    });
                });
            }
            return userVmList;
        }

        public bool UserLock(int userId)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            if(user != null)
            {
                user.Locked = true;
                _ppsDbContext.User.Attach(user);
                _ppsDbContext.Entry(user).State = EntityState.Modified;
                _ppsDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool UserUnlock(int userId)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.Locked = false;
                _ppsDbContext.User.Attach(user);
                _ppsDbContext.Entry(user).State = EntityState.Modified;
                _ppsDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool UserActivate(int userId)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.StatusId = (int)UserStatusEnum.Active;
                _ppsDbContext.User.Attach(user);
                _ppsDbContext.Entry(user).State = EntityState.Modified;
                _ppsDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool UserDeactivate(int userId)
        {
            var user = _ppsDbContext.User.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.StatusId = (int)UserStatusEnum.Inactive;
                _ppsDbContext.User.Attach(user);
                _ppsDbContext.Entry(user).State = EntityState.Modified;
                _ppsDbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Register(UserRegisterVm user)
        {
            var aspNetUser = _ppsDbContext.AspNetUsers.FirstOrDefault(x => x.Email == user.Email);
            var newUser = new User
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,                
                Phone = "",
                CompanyId = user.CompanyId,
                EmployeeId = user.EmployeeId,
                StatusId = (int)UserStatusEnum.Active,
                Locked = false,
                InvalidCount = 0,
                Password = user.Password,
                PasswordKey = user.PasswordKey,
                AspNetUserId = aspNetUser.Id
            };
            _ppsDbContext.User.Add(newUser);
            _ppsDbContext.SaveChanges();
            //var createdUser = _ppsDbContext.User.FirstOrDefault(x => x.Id == newUser.Id);
            //var createdUserDto = new UserDto
            //{
            //    Id = createdUser.Id,
            //    Email = createdUser.Email,
            //    Name = StringExtension.ToFullName(createdUser.FirstName, createdUser.LastName),
            //    Locked = createdUser.Locked == true ? "Locked" : "",
            //    Status = createdUser.UserStatus.Status
            //};
            //return createdUserDto;
            return true;
        }

        public async Task<UserVm> GetUserRoleById(int userId)
        {
            var user = await _ppsDbContext.User.FirstOrDefaultAsync(x => x.Id == userId);
            if(user == null)
            {
                throw new Exception("User doesn't exist.");
            }
            var roles = user.UserRole.ToList();
            var userVm = new UserVm
            {
                Id = user.Id,
                Name = StringExtension.ToFullName(user.FirstName, user.LastName),
                Email = user.Email
            };
            
            return userVm;
        }

        public async Task<bool> UpdateUser(UserVm userVm)
        {
            using (var db = new PPSDbContext())
            {
                if (userVm.Id == 0)
                {
                    throw new InvalidOperationException();
                }
                var user = db.User.FirstOrDefault(x => x.Id == userVm.Id);

                if (user == null)
                {
                    throw new Exception($"The user id: {userVm.Id} doesn't exist.");
                }

                var userRole = user.UserRole.ToList();
                userRole.ForEach(ur =>
                {
                    db.UserRoleHistory.Add(new UserRoleHistory
                    {
                        UserRoleId = ur.Id,
                        UserId = ur.UserId,
                        RoleId = ur.RoleId,
                        AssignedBy = ur.AssignedBy,
                        AssignedOn = ur.AssignedOn
                    });
                });

                db.UserRole.RemoveRange(user.UserRole);
                var newUserRoles = new ConcurrentBag<UserRole>();
                userVm.Roles.ForEach(role =>
                {
                    newUserRoles.Add(new UserRole
                    {
                        RoleId = role.Id,
                        UserId = userVm.Id,
                        AssignedBy = userVm.AssignedUserId,
                        AssignedOn = DateTime.Now
                    });
                });
                db.UserRole.AddRange(newUserRoles);
                await db.SaveChangesAsync();
            }
            return true;
        }

        public IQueryable<User> GetUsers()
        {
            return _ppsDbContext.User;
        }
    }
}