using PPS.Data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPS.Data.Dtos.User;
using PPS.Data.Edmx;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using PPS.API.Shared.ViewModel.User;
using System.Data.Entity;

namespace PPS.Data.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private PPSDbContext _ppsDbContext;
        public PolicyRepository()
        {
            _ppsDbContext = new PPSDbContext();
        }

        //public async Task<bool> AddRole(RoleVm roleVm)
        //{
        //    var role = new Role
        //    {
        //        RoleName = roleVm.RoleName,
        //        Description = roleVm.Description
        //    };
        //    _ppsDbContext.Role.Add(role);
        //    await _ppsDbContext.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<List<RoleDto>> GetUserRoles()
        //{
        //    var roles = _ppsDbContext.Role.ToList();
        //    var roleDtoList = new ConcurrentBag<RoleDto>();
        //    if (roles.Count > 0)
        //    {
        //        roles.ForEach(u =>
        //        {
        //            roleDtoList.Add(new RoleDto
        //            {
        //                Id = u.Id,
        //                RoleName = u.RoleName,
        //                Description = u.Description
        //            });
        //        });
        //    }
        //    return roleDtoList.ToList();
        //}

        public async Task<List<PolicyDto>> GetPolicyByRole(int roleId,int status)
        {
            var role = await _ppsDbContext.Role.FirstOrDefaultAsync(x => x.Id == roleId);
            var policies = _ppsDbContext.Policy.Where(m=>m.AppTypeId==status).ToList();

            var policyDtoList = new ConcurrentBag<PolicyDto>();
            if (policies.Count > 0)
            {
                policies.ForEach(u =>
                {
                    policyDtoList.Add(new PolicyDto
                    {
                        Id = u.Id,
                        PolicyName = u.PolicyName,
                        Description = u.Description,
                        AppTypeId=u.AppTypeId,
                        Selected = false
                    });
                });
            }
            var rolePolicies = role.RolePolicy.ToList();
            if (rolePolicies.Count > 0&& policyDtoList.Count>0)
            {
                rolePolicies.ForEach(p =>
                {
                     policyDtoList.FirstOrDefault(x => x.Id == p.PolicyId).Selected = true;
                });
            }
            return policyDtoList.ToList();
        }

        public async Task<List<PolicyDto>> GetPolicyByUser(string aspNetUserId)
        {
            var user = await _ppsDbContext.User.FirstOrDefaultAsync(x => x.AspNetUserId == aspNetUserId);
            var policyDtoList = new ConcurrentBag<PolicyDto>();
            var userRole = _ppsDbContext.UserRole.Where(x => x.UserId == user.Id).ToList();

            userRole.ForEach(uRole =>
            {
                var rolePolicy = _ppsDbContext.RolePolicy.Where(x => x.RoleId == uRole.RoleId).ToList();
                rolePolicy?.ForEach(p =>
                {
                    policyDtoList.Add(new PolicyDto
                    {
                        Id = p.PolicyId,
                        PolicyName = p.Policy?.PolicyName,
                        PolicyCode = (int)p.Policy?.PolicyCode,
                        AppTypeId=p.Policy.AppTypeId,
                        Description = p.Policy?.Description,
                    });
                });
            });

            return policyDtoList.ToList();
        }

        public async Task<bool> UpdateRolePolicy(RolePolicyVm rolePolicyVm, int userId)
        {
            using (var db = new PPSDbContext())
            {
                if (rolePolicyVm != null)
                {
                    if (rolePolicyVm.Id == 0)
                        throw new InvalidOperationException();

                    var role = await db.Role.FirstOrDefaultAsync(x => x.Id == rolePolicyVm.Id);
                    var policies = role.RolePolicy.ToList();
                    if (rolePolicyVm.Policies.Count > 0)
                    {
                        policies.ForEach(p =>
                        {
                            db.RolePolicyHistory.Add(new RolePolicyHistory
                            {
                                RolePolicyId = p.Id,
                                RoleId = p.RoleId,
                                PolicyId = p.PolicyId,
                                AssignedBy = p.AssignedBy,
                                AssignedOn = p.AssignedOn
                            });
                        });

                        db.RolePolicy.RemoveRange(policies);
                        var selectedPolicies = rolePolicyVm.Policies.Where(x => x.Selected == true).ToList();
                        selectedPolicies.ForEach(p =>
                        {
                            db.RolePolicy.Add(new RolePolicy
                            {
                                RoleId = role.Id,
                                PolicyId = p.Id,
                                AssignedBy = userId,
                                AssignedOn = DateTime.Now
                            });
                        });
                        await db.SaveChangesAsync();
                    }
                }
                return true;
            }
        }

        public bool ValidateUsersAccess(string aspNetUserId, string controller, string action)
        {
            //var routeResource = _ppsDbContext.RouteResource.AsNoTracking().FirstOrDefault(x => x.ControllerName == controller && x.ActionName == action);
            //if (routeResource == null)
            //{
            //    return false;
            //}
            var hasPolicy = _ppsDbContext.User.Any(z => z.AspNetUserId == aspNetUserId && z.UserRole.Any(x => x.Role.RolePolicy.Any(k => k.Policy.RouteResource.Any(g => g.ControllerName == controller && g.ActionName == action))));
            //var userRolePolicy = user.UserRole.Any(x => x.Role.RolePolicy.Any(k => k.Policy.RouteResource.Any(g => g.ControllerName == controller && g.ActionName == action)));
            //var policyDtoList = new ConcurrentBag<PolicyDto>();
            //var userRole = user.UserRole.ToList();
            
            //var hasPolicy = false;
            //userRole.ForEach(uRole =>
            //{
            //    var rolePolicy = uRole?.Role?.RolePolicy.ToList();
            //    var policyRouteResource = rolePolicy.SelectMany(x => x.Policy.RouteResource);

            //    if (policyRouteResource != null && policyRouteResource.Where(x => x.Id == routeResource.Id).Count() > 0)
            //    {
            //        hasPolicy = true;
            //        return;
            //    }
            //});
            return hasPolicy;
        }
    }
}