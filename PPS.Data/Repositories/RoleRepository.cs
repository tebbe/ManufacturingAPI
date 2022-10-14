using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos.User;
using PPS.Data.Edmx;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.User;

namespace PPS.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private PPSDbContext _ppsDbContext;
        public RoleRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        public async Task<bool> AddRole(RoleVm roleVm)
        {
            var role = new Role
            {
                RoleName = roleVm.RoleName,
                Description = roleVm.Description
            };
            _ppsDbContext.Role.Add(role);
            await _ppsDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RoleDto> GetRoleById(int roleId)
        {
            var role = await _ppsDbContext.Role.FirstOrDefaultAsync(x => x.Id == roleId);
            var roleDto = new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description
            };
            return roleDto;
        }

        public async Task<List<RoleDto>> GetUserRoles()
        {
            var roles = await _ppsDbContext.Role.ToListAsync();
            var roleDtoList = new ConcurrentBag<RoleDto>();
            if (roles.Count > 0)
            {
                roles.ForEach(u =>
                {
                    roleDtoList.Add(new RoleDto
                    {
                        Id = u.Id,
                        RoleName = u.RoleName,
                        Description = u.Description
                    });
                });
            }
            return roleDtoList.ToList();
        }

        public async Task<List<RoleDto>> GetRoleByUserId(int userId)
        {
            var userRoles =await _ppsDbContext.UserRole.Where(x => x.UserId == userId).ToListAsync();
            var roles = await _ppsDbContext.Role.ToListAsync();

            var roleDtoList = new ConcurrentBag<RoleDto>();
            if (roles.Count > 0)
            {
                roles.ForEach(role =>
                {
                    roleDtoList.Add(new RoleDto
                    {
                        Id = role.Id,
                        RoleName = role.RoleName,
                        Description = role.Description,
                        Selected = false
                    });
                });
            }
            userRoles.ToList().ForEach(p =>
            {
                roleDtoList.FirstOrDefault(x => x.Id == p.RoleId).Selected = true;
            });
            return roleDtoList.ToList();
        }        
    }
}