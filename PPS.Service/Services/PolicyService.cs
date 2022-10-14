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
    public class PolicyService : IPolicyService
    {
        private IPolicyRepository _policyRepository;
        private readonly IRoleRepository _roleRepository;
        public PolicyService()
        {
            _policyRepository = new PolicyRepository();
            _roleRepository = new RoleRepository();
        }

        public async Task<RolePolicyVm> GetPolicyByRole(int roleId,int status)
        {
            var policies = await _policyRepository.GetPolicyByRole(roleId,status);
            var policiesVm = policies.Select(x => x.ToPolicyVm()).ToList();
            var role = await _roleRepository.GetRoleById(roleId);
            var vm = new RolePolicyVm
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                Policies = policiesVm
            };
            return vm;
        }

        public async Task<List<PolicyVm>> GetPolicyByUser(string aspNetUserId)
        {
            var policies = await _policyRepository.GetPolicyByUser(aspNetUserId);
            var policiesVm = policies.Select(x => x.ToPolicyVm()).ToList();
            return policiesVm;
        }

        public async Task<bool> UpdateRolePolicy(RolePolicyVm rolePolicyVm, int userId)
        {
            return await _policyRepository.UpdateRolePolicy(rolePolicyVm, userId);
        }

        public bool ValidateUsersAccess(string aspNetUserId, string controller, string action, string httpMethod)
        {
            //var userPolicy = GetPolicyByUser(aspNetUserId);
            return  _policyRepository.ValidateUsersAccess(aspNetUserId, controller, action);
        }
    }
}