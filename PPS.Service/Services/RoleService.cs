using PPS.Service.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.API.Shared.ViewModel.User;
using PPS.Data.RepositoryInterfaces;
using PPS.Data.Repositories;
using System.Threading.Tasks;

namespace PPS.Service.Services
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _roleRepository;
        public RoleService()
        {
            _roleRepository = new RoleRepository();
        }

        public async Task<bool> AddRole(RoleVm roleVm)
        {
            return await _roleRepository.AddRole(roleVm);
        }

        public async Task<List<RoleVm>> GetRoleByUserId(int userId)
        {
            var roles = await _roleRepository.GetRoleByUserId(userId);
            var rolesVm = roles.Select(x => x.ToRoleVm());
            return rolesVm.ToList();
        }

        public async Task<List<RoleVm>> GetUserRoles()
        {
            var roles = await _roleRepository.GetUserRoles();
            var rolesVm = roles.Select(x => x.ToRoleVm());
            return rolesVm.ToList();
        }        
    }
}