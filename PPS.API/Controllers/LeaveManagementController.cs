using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel;
using PPS.API.Shared.ViewModel.Leave;
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
    [RoutePrefix("api/LeaveManagement")]
    public class LeaveManagementController : BaseApiController
    {
      
        private readonly ILeaveInterface _leaveService;
        private readonly ILogger _logger;
        public LeaveManagementController()
        {
            _leaveService = new LeaveService();
            _logger = new Logger();

        }

        [Route("GetEmployeeLeaveList")]
        [HttpGet]
        public IHttpActionResult GetEmployeeLeaveList()
        {
            try
            {
                var leaveList = _leaveService.GetEmployeeLeaveList();
                return Ok(leaveList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeAndEmployeeHierArchyLeaveList/{status}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeAndEmployeeHierArchyLeaveList(int? status)
        {
            try
            {
              var userVm = await UserId();
           
                var  employeeLeaveList = _leaveService.GetLeaveListByEmployeeId(userVm.EmployeeId);
                var employeeDetails = _leaveService.GetEmployeeLeaveDetailsByEmployeeId(userVm.EmployeeId);
                var employeeHierArchyLeaveList = _leaveService.GetEmployeeHierArchyLeaveList(userVm.EmployeeId,status);

                return Ok(new {EmployeeLeaveList = employeeLeaveList, EmployeeDetails = employeeDetails,EmployeeHierArchyLeaveList= employeeHierArchyLeaveList });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeLeaveCategoryWithHierArchy")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeLeaveCategoryWithHierArchy()
        {
            try
            {
                var userVm = await UserId();
                var leaveCategory = _leaveService.GetLeaveCategory();
                var employeeHierArchy = _leaveService.GetEmployeeHierArchy(userVm.EmployeeId);
                if (employeeHierArchy.Count()==0)
                {
                  employeeHierArchy = _leaveService.GetEmployeeById(userVm.EmployeeId);
                }
               
                return Ok(new { LeaveCategory= leaveCategory, EmployeeHierArchy= employeeHierArchy });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveEmployeeLeave")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveEmployeeLeave(EmployeeLeaveVm employeeLeaveVm)
        {
            try
            {
                var userVm = await UserId();
                employeeLeaveVm.CreatedBy = userVm.Id;
                employeeLeaveVm.CreatedOn = DateTime.Now;
                employeeLeaveVm.DateOfApplication = DateTime.Now;
                employeeLeaveVm.LeaveYear = DateTime.Now.Year;
                var saveLeave = _leaveService.SaveEmployeeLeave(employeeLeaveVm);
                return Ok(saveLeave);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeLeaveById/{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeLeaveById(int Id)
        {
            try
            {
                var userVm = await UserId();
                var getLeaveById = _leaveService.GetEmployeeLeaveById(Id);
                var leaveCategory= _leaveService.GetLeaveCategory();
                var employeeHierArchy = _leaveService.GetEmployeeHierArchy(userVm.EmployeeId);
                if (employeeHierArchy.Count() == 0)
                {
                    employeeHierArchy = _leaveService.GetEmployeeById(userVm.EmployeeId);
                }
                return Ok(new {LeaveById= getLeaveById,LeaveCategory= leaveCategory,EmployeeHierArchy= employeeHierArchy });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateEmployeeLeave")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateEmployeeLeave(EmployeeLeaveVm employeeLeaveVm)
        {
            try
            {
                var userVm = await UserId();
                employeeLeaveVm.UpdatedBy = userVm.Id;
                employeeLeaveVm.UpdatedOn = DateTime.Now;
                employeeLeaveVm.DateOfApplication = DateTime.Now;
                var updateLeave = _leaveService.UpdateEmployeeLeave(employeeLeaveVm);
                return Ok(updateLeave);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("EmployeeLeaveApproveOrReject")]
        [HttpPost]
        public async Task<IHttpActionResult> EmployeeLeaveApproveOrReject(EmployeeLeaveVm employeeLeaveVm)
        {
            try
            {
                var userVm = await UserId();
                var  leaveStatus = _leaveService.ApproveOrRejectEmployeeLeave(employeeLeaveVm);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
