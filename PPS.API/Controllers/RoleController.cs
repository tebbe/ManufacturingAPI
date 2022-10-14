using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel.User;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Role")]
    public class RoleController : BaseApiController
    {
        private IRoleService _roleService;
        private IPolicyService _policyService;
        PPS.Shared.Core.HelperClasses.ILogger _logger;

        public RoleController()
        {
            _roleService = new RoleService();
            _policyService = new PolicyService();
            _logger = new Logger();
        }
        [Route("GetUserRoles")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserRoles()
        {
            try
            {
                var users = await _roleService.GetUserRoles();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("AddRole")]
        [HttpPost]
        public async Task<IHttpActionResult> AddRole(RoleVm roleVm)
        {
            try
            {
                await _roleService.AddRole(roleVm);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetPolicyByRole/{roleId}/{appTypeStatus}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPolicyByRole(int roleId,int appTypeStatus)
        {
            try
            {
                var rolePolicies = await _policyService.GetPolicyByRole(roleId,appTypeStatus);
                return Ok(rolePolicies);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetPolicyByUser/{aspNetUserId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPolicyByUser(string aspNetUserId)
        {
            try
            {
                var userPolicies = await _policyService.GetPolicyByUser(aspNetUserId);
                return Ok(userPolicies);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("UpdateRolePolicy")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateRolePolicy(RolePolicyVm rolePolicyVm)
        {
            try
            {
                var userVm = await UserId();
                await _policyService.UpdateRolePolicy(rolePolicyVm, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
