using PPS.Data.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.User;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IRoleRepository
    {
        Task<List<RoleDto>> GetUserRoles();
        Task<bool> AddRole(RoleVm roleVm);
        Task<RoleDto> GetRoleById(int roleId);
        Task<List<RoleDto>> GetRoleByUserId(int userId);        
    }
}
