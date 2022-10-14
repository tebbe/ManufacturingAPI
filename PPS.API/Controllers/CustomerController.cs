using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel.Sales;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.ViewModel.Customer;
using PPS.Shared.Core.HelperClasses;
using PPS.API.Shared.Enums;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Customer")]
    public class CustomerController : BaseApiController
    {
        ICustomerInterface _customerSvc;
        ILogger _logger;
        public CustomerController()
        {
            _customerSvc = new CustomerService();
            _logger = new Logger();
        }

        [Route("GetCustomerList")]
        [HttpGet]
        public IHttpActionResult GetCustomerList()
        {
            try
            {
                var customerList = _customerSvc.GetCustomerList();
                return Ok(customerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetAttachmentType")]
        [HttpGet]
        public IHttpActionResult GetAttachmentType()
        {
            try
            {
                var attachmentTypeList = _customerSvc.GetAttachmentType();
                return Ok(attachmentTypeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetPendingDeactivatedCustomerList")]
        [HttpGet]
        public IHttpActionResult GetPendingDeactivatedCustomerList()
        {
            try
            {
                var customerList = _customerSvc.GetPendingDeactivatedCustomerList();
                return Ok(customerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetCustomerById/{customerId}")]
        [HttpGet]
        public IHttpActionResult GetCustomerById(int customerId)
        {
            try
            {
                var customerVm = _customerSvc.GetCustomerById(customerId);
                return Ok(customerVm);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveCustomer")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveCustomer(CustomerModel customerModel)
        {
            try
            {
                var userVm = await UserId();
                //customerModel.AccountHeadId = null;
                customerModel.CreatedBy = userVm.Id;
                customerModel.CreatedOn = DateTime.Today.Date;
                var newCustomer = _customerSvc.SaveCustomer(customerModel);
                return Ok(newCustomer);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("UpdateCustomer")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCustomer(CustomerModel customerModel)
        {
            try
            {
                var userVm = await UserId();
                customerModel.UpdatedBy = userVm.Id;
                customerModel.UpdatedOn = DateTime.Today.Date;
                var customer = _customerSvc.UpdateCustomer(customerModel);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        //[Route("VerifyCustomer/{customerId}")]
        //[HttpPost]
        //public async Task<IHttpActionResult> VerifyCustomer(int customerId)
        //{
        //    try
        //    {
        //        var userVm = await UserId();
        //        var customerModel = new CustomerModel
        //        {
        //            Id = customerId,
        //            CreatedBy = userVm.Id,
        //            CreatedOn = DateTime.Today.Date
        //        };
        //        var verified = await _customerSvc.VerifyCustomerAsync(customerModel);
        //        return Ok(verified);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}
        [Route("DeactivateCustomer/{customerId}")]
        [HttpPost]
        public async Task<IHttpActionResult> DeactivateCustomer(int customerId)
        {
            try
            {
                var userVm = await UserId();
                var customerModel = new CustomerModel
                {
                    Id = customerId,
                    CreatedBy = userVm.Id,
                    CreatedOn = DateTime.Today.Date
                };
                var deactivated = await _customerSvc.DeactivateCustomerAsync(customerModel);
                return Ok(deactivated);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ActivateCustomer/{customerId}")]
        [HttpPost]
        public async Task<IHttpActionResult> ActivateCustomer(int customerId)
        {
            try
            {
                var userVm = await UserId();
                var customerModel = new CustomerModel
                {
                    Id = customerId,
                    CreatedBy = userVm.Id,
                    CreatedOn = DateTime.Today.Date
                };
                var activated = await _customerSvc.ActivateCustomerAsync(customerModel);
                return Ok(activated);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveCustomerTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveCustomerTransaction(CustomerTransactionVm customerTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                customerTransactionVm.CreatedBy = userVm.Id;
                customerTransactionVm.CreatedOn = DateTime.Now;
                var createdCustomerTransaction = await _customerSvc.SaveCustomerTransaction(userVm.Id, customerTransactionVm);
                return Ok(createdCustomerTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("ApproveCustomerTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveCustomerTransaction(CustomerTransactionVm customerTransactionVm, int fiscalYear, int companyId)
        {
            try
            {
                var userVm = await UserId();
                customerTransactionVm.ApprovedBy = userVm.Id;
                customerTransactionVm.ApprovedOn = DateTime.Now;
                userVm.CompanyId = companyId;
                var approveCustomerTransaction = _customerSvc.ApproveCustomerTransaction(userVm, fiscalYear, customerTransactionVm);
                return Ok(new { Id = customerTransactionVm.Id });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetUnapprovedCustomerTransaction")]
        [HttpGet]
        public IHttpActionResult GetUnapprovedCustomerTransaction()
        {
            try
            {
                var customerVm = _customerSvc.GetUnapprovedCustomerTransaction();
                return Ok(customerVm);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetAvailableBalanceByCustomerId/{customerId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAvailableBalanceByCustomerId(int customerId)
        {
            try
            {
                var customerAvailableBalance = await _customerSvc.GetAvailableBalanceByCustomerId(customerId);
                return Ok(customerAvailableBalance);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetCustomerType")]
        [HttpGet]
        public IHttpActionResult GetCustomerType()
        {
            try
            {
                var customerTypeList = _customerSvc.GetCustomerType();
                return Ok(customerTypeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetCustomerTransactionList")]
        [HttpGet]
        public IHttpActionResult GetCustomerTransactionList()
        {
            try
            {
                var customerTransactionList = _customerSvc.GetCustomerTransactionList();
                return Ok(customerTransactionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetCustomerTransactionListForFiltering")]
        [HttpGet]
        public IHttpActionResult GetCustomerTransactionListForFiltering(int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            try
            {
                var filterVm = new FilterVm
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = SortDirectionEnum.ASC.ToString() == sortDirection ? SortDirectionEnum.ASC : SortDirectionEnum.DESC
                };
                var customerTransactionList = _customerSvc.GetCustomerTransactionListForFiltering(filterVm);
                return Ok(customerTransactionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetUnapprovedCustomerTransactionForFiltering")]
        [HttpGet]
        public IHttpActionResult GetUnapprovedCustomerTransactionForFiltering(int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            try
            {
                var filterVm = new FilterVm
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = SortDirectionEnum.ASC.ToString() == sortDirection ? SortDirectionEnum.ASC : SortDirectionEnum.DESC
                };
                var customerTransactionList = _customerSvc.GetUnapprovedCustomerTransactionForFiltering(filterVm);
                return Ok(customerTransactionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("customerTransactionSearch/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult CustomerTransactionSearch(DateTime startDate, DateTime endDate)
        {
            try
            {
                var customerTransactionList = _customerSvc.CustomerTransactionSearch(startDate, endDate);
                return Ok(customerTransactionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetCustomerTransactionByTransactionId/{Id}")]
        [HttpGet]
        public IHttpActionResult GetCustomerTransactionByTransactionId(int id)
        {
            try
            {
                var customerTransaction = _customerSvc.GetCustomerTransactionByTransactionId(id);
                return Ok(customerTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateCustomerTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCustomerTransaction(CustomerTransactionVm customerTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                customerTransactionVm.UpdatedBy = userVm.Id;
                customerTransactionVm.UpdatedOn = DateTime.Now;
                var updatedCustomerTransaction =  _customerSvc.UpdateCustomerTransaction(userVm.Id, customerTransactionVm);
                return Ok(updatedCustomerTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
