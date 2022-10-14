using PPS.API.Shared.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface IRoleService
    {
        Task<List<RoleVm>> GetUserRoles();
        Task<bool> AddRole(RoleVm roleVm);
        Task<List<RoleVm>> GetRoleByUserId(int userId);        
    }
}
