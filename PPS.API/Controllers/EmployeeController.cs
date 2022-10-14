using PPS.API.HelperClasses;
using PPS.API.Shared.Enums;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ViewModel.Employee;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel.Sales;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : BaseApiController
    {
        IEmployeeInterface _employeeSvc;
        ILogger _logger;
        public EmployeeController()
        {
            _employeeSvc = new EmployeeService();
            _logger = new Logger();
        }

        [Route("GetAllEmployee")]
        [HttpGet]
        public IHttpActionResult GetAllEmployee()
        {
            try
            {
                var employeeList = _employeeSvc.GetEmployee();
                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesEmployee")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesEmployee()
        {
            try
            {
                var vm = await UserId();
                var employeeList = _employeeSvc.GetEmployee(vm.EmployeeId);
                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesEmployeeWithSalesTargetByMonth/{year}/{month}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesEmployeeWithSalesTargetByMonth(int year, int month)
        {
            try
            {
                var employeeRequestVm = new EmployeeRequestVm { Year = year, Month = month };
                var vm = await UserId();
                var employeeList = _employeeSvc.GetSalesEmployeeWithSalesTargetByMonth(employeeRequestVm);
                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesHierarchyById/{employeeId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesHierarchyById(int employeeId = 0)
        {
            try
            {
                if (employeeId == 0)
                {
                    var vm = await UserId();
                    employeeId = vm.EmployeeId;
                }

                var empHierarchy = _employeeSvc.GetEmployeeHierarchy(employeeId);
                return Ok(empHierarchy);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetActiveEmployeeList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetActiveEmployeeList()
        {
            try
            {
                var vm = await UserId();
                var employeeList = _employeeSvc.GetActiveEmployeeList();
                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetInactiveEmployeeList")]
        [HttpGet]

        public async Task<IHttpActionResult> GetInactiveEmployeeList()
        {
            try
            {
                var vm = await UserId();
                var employeeList = _employeeSvc.GetInactiveEmployeeList();
                return Ok(employeeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeDropDownList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeDropDownList()
        {
            try
            {
                var vm = await UserId();
                var employeeType = _employeeSvc.GetEmployeeType();
                var managerList = _employeeSvc.GetManager();
                var deptList = _employeeSvc.GetDepartment();
                var desigList = _employeeSvc.GetDesignation();
                var salesDivisionList = _employeeSvc.GetSalesDivision();
                return Ok(new { dept = deptList, man = managerList, desi = desigList, div = salesDivisionList,EmployeeType= employeeType });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeAreaList/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeAreaList(int id)
        {
            try
            {
                var vm = await UserId();
                var salesAreaList = _employeeSvc.GetSalesAreaByDevisionId(id);
                return Ok(salesAreaList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeBaseList/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeBaseList(int id)
        {
            try
            {
                var vm = await UserId();
                var salesBaseList = _employeeSvc.GetSalesBaseByAreaId(id);
                return Ok(salesBaseList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("AddNewEmployee")]
        [HttpPost]
        public async Task<IHttpActionResult> AddNewEmployee(EmployeeVm employeeVm)
        {
            try
            {
                var vm = await UserId();
                employeeVm.CreatedBy = vm.Id;
                employeeVm.CreatedOn = DateTime.Now;
                var addEmployee = _employeeSvc.AddNewEmployee(employeeVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetEmployeeById/{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEmployeeById(int Id)
        {
            try
            {
                var vm = await UserId();
                var employee = _employeeSvc.GetEmployeeById(Id);
                var salesAreaList = new List<SalesAreaVm>();
                var salesBaseList = new List<SalesBaseVm>();
                var salesLocationList = new List<EmployeeSalesLocationVm>();
                if (employee.Id > 0)
                {
                    salesLocationList = _employeeSvc.GetSalesLocationByEmployeeId(employee.Id);
                }
                if (employee.SalesDivisionId != null)
                {
                    salesAreaList = _employeeSvc.GetSalesAreaByDevisionId((int)employee.SalesDivisionId);
                }
                if (employee.SalesBaseId != null)
                {
                    salesBaseList = _employeeSvc.GetSalesBaseByAreaId((int)employee.SalesAreaId);
                }
                var areaList = _employeeSvc.GetSalesAreaList();
                var baseList = _employeeSvc.GetSalesBaseList();
                return Ok(new { Employee = employee, SalesAreaList = salesAreaList, SalesBaseList = salesBaseList, sLocation = salesLocationList, AreaList = areaList, BaseList = baseList });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("EmployeeUpdate")]
        [HttpPost]
        public async Task<IHttpActionResult> EmployeeUpdate(EmployeeVm employeeVm)
        {
            try
            {
                var userVm = await UserId();
                employeeVm.UpdatedBy = userVm.Id;
                employeeVm.UpdatedOn = DateTime.Now;
                employeeVm.CreatedBy = userVm.Id;
                employeeVm.CreatedOn = DateTime.Now;
                var editEmployeee = _employeeSvc.EmployeeUpdate(employeeVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("EmployeeListPrint/{status}")]
        [HttpGet]
        public async Task<IHttpActionResult> EmployeeListPrint(bool status)
        {
            try
            {
                var vm = await UserId();
                var employee = status == false ? _employeeSvc.GetInactiveEmployeeList() : _employeeSvc.GetActiveEmployeeList();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("EmployeeHistoryList/{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> EmployeeHistoryList(int Id)
        {
            try
            {
                var vm = await UserId();
                var employeeDetails = _employeeSvc.GetEmployeeById(Id);
                var employeeHistory = _employeeSvc.GetEmployeeHistory(Id);

                return Ok(new { EmployeeDetails = employeeDetails, empHistory = employeeHistory});
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        //Employee Sales Locations start 
        [Route("GetEmployeeSalesLocation")]
        [HttpGet]
        public IHttpActionResult GetEmployeeSalesLocation()
        {
            try
            {
                var salesDivisionList = _employeeSvc.GetSalesDivision();
                var salesAreaList = _employeeSvc.GetSalesAreaList();
                var salesBaseList = _employeeSvc.GetSalesBaseList();
                return Ok(new { div = salesDivisionList, area = salesAreaList, sBase = salesBaseList });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("AddSalesArea")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSalesArea(SalesArea salesArea)
        {
            try
            {
                var userVm = await UserId();
                var addSalesArea = _employeeSvc.AddSalesArea(salesArea);
                return Ok(addSalesArea);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("AddSalesBase")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSalesBase(SalesBase salesBase)
        {
            try
            {
                var userVm = await UserId();
                var addSalesBase = _employeeSvc.AddSalesBase(salesBase);
                return Ok(addSalesBase);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesAreaById/{Id}")]
        [HttpGet]
        public IHttpActionResult GetSalesAreaById(int Id)
        {
            try
            {
                var salesAre = _employeeSvc.GetSalesAreaById(Id);
                return Ok(salesAre);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetSalesBaseById/{Id}")]
        [HttpGet]
        public IHttpActionResult GetSalesBaseById(int Id)
        {
            try
            {
                var salesBase = _employeeSvc.GetSalesBaseById(Id);
                return Ok(salesBase);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateSalesArea")]
        [HttpPost]
        public IHttpActionResult UpdateSalesArea(SalesArea salesArea)
        {
            try
            {
                var editSalesArea = _employeeSvc.UpdateSalesArea(salesArea);
                return Ok(editSalesArea);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("UpdateSalesBase")]
        [HttpPost]
        public IHttpActionResult UpdateSalesBase(SalesBase salesBase)
        {
            try
            {
                var editSalesBase = _employeeSvc.UpdateSalesBase(salesBase);
                return Ok(editSalesBase);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetEmployeeSalesLocationByEmployeeId/{Id}")]
        [HttpGet]
        public IHttpActionResult GetEmployeeSalesLocationByEmployeeId(int Id)
        {
            try
            {
                var employeeSalesLocationList = _employeeSvc.GetSalesLocationByEmployeeId(Id);
                return Ok(new { SalesLocationByEmployeeId = employeeSalesLocationList });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        
    }
}

