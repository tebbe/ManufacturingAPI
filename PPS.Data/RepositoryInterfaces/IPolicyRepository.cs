using PPS.Data.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.User;

namespace PPS.Data.RepositoryInterfaces
{
    public interface IPolicyRepository
    {
        Task<List<PolicyDto>> GetPolicyByRole(int roleId,int status);
        Task<bool> UpdateRolePolicy(RolePolicyVm rolePolicyVm, int userId);
        Task<List<PolicyDto>> GetPolicyByUser(string aspNetUserId);
        bool ValidateUsersAccess(string aspNetUserId, string controller, string action);
    }
}
