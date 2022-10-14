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
    public class PolicyController : BaseApiController
    {
        private IPolicyService _policyService;        
        PPS.Shared.Core.HelperClasses.ILogger _logger;

        public PolicyController()
        {
            _policyService = new PolicyService();
            _logger = new Logger();
        }
        //[Route("GetPolicyByRole/{roleId}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetPolicyByRole(int roleId)
        //{
        //    try
        //    {
        //        var rolePolicies = await _policyService.GetPolicyByRole(roleId);
        //        return Ok(rolePolicies);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}
        //[Route("GetPolicyByUser/{aspNetUserId}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetPolicyByUser(string aspNetUserId)
        //{
        //    try
        //    {
        //        var userPolicies = await _policyService.GetPolicyByUser(aspNetUserId);
        //        return Ok(userPolicies);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}
        //[Route("UpdateRolePolicy")]
        //[HttpPost]
        //public async Task<IHttpActionResult> UpdateRolePolicy(RolePolicyVm rolePolicyVm)
        //{
        //    try
        //    {
        //        await _policyService.UpdateRolePolicy(rolePolicyVm);
        //        return Ok(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}

    }
}
