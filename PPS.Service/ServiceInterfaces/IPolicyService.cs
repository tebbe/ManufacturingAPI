using PPS.API.Shared.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPS.Service.ServiceInterfaces
{
    public interface IPolicyService
    {
        Task<RolePolicyVm> GetPolicyByRole(int roleId,int status);
        Task<bool> UpdateRolePolicy(RolePolicyVm rolePolicyVm, int userVmId);
        Task<List<PolicyVm>> GetPolicyByUser(string aspNetUserId);
        bool ValidateUsersAccess(string aspNetUserId, string controller, string action, string httpMethod);
    }
}
