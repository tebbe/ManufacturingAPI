using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel.Sales;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.RequestVm;
using PPS.Shared.Core.HelperClasses;
using PPS.API.Shared.Enums;
using PPS.Data.Edmx;
using PPS.API.Shared.ViewModel.Filter;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Sales")]
    public class SalesController : BaseApiController
    {
        readonly ISalesInterface _salesSvc;
        readonly ICompanyService _companyService;
        readonly ILogger _logger;
        public SalesController()
        {
            _salesSvc = new SalesService();
            _logger = new Logger();
            _companyService = new CompanyService();
        }


        [Route("GetSaleType")]
        [HttpGet]
        public IHttpActionResult GetSaleType()
        {
            try
            {
                var saleTypeList = _salesSvc.GetSaleType();
                return Ok(saleTypeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderType")]
        [HttpGet]
        public IHttpActionResult GetDemandOrderType()
        {
            try
            {
                var demandOrderTypeList = _salesSvc.GetDemandOrderType();
                return Ok(demandOrderTypeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDiscountType")]
        [HttpGet]
        public IHttpActionResult GetDiscountType()
        {
            try
            {
                var discountTypeList = _salesSvc.GetDiscountType();
                return Ok(discountTypeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderList()
        {
            try
            {
                var userVm = await UserId();
                int paymentStatus = (int)PaymentStatusEnum.All;
                var demandOrderList = _salesSvc.GetDemandOrderList(userVm.Id, paymentStatus);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderListForFiltering")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderListForFiltering(int pageIndex, int pageSize, string sortColumn, string sortDirection,DateTime? StartDate,DateTime? EndDate,int? CustomerId)
        {
            try
            {
                var filterVm = new FilterVm {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = SortDirectionEnum.ASC.ToString() == sortDirection? SortDirectionEnum.ASC : SortDirectionEnum.DESC,
                    StartDate=StartDate,
                    EndDate=EndDate,
                    CustomerId=CustomerId
                };

                PPS.Logging.Log.Default.Info($"StartDate : {filterVm.StartDate}, EndDate : {filterVm.EndDate}, CustomerId : {filterVm.CustomerId}");

                var userVm = await UserId();
                int paymentStatus = (int)PaymentStatusEnum.All;
                var demandOrderList = _salesSvc.GetDemandOrderListForFiltering(userVm.Id, paymentStatus, filterVm);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderUnPaidList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderUnPaidList()
        {
            try
            {
                var userVm = await UserId();
                int paymentStatus = (int)PaymentStatusEnum.Unpaid;
                var demandOrderList = _salesSvc.GetDemandOrderList(userVm.Id, paymentStatus);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetDemandOrderPartiallyPaidList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderPartiallyPaidList()
        {
            try
            {
                var userVm = await UserId();
                int paymentStatus = (int)PaymentStatusEnum.PartiallyPaid;
                var demandOrderList = _salesSvc.GetDemandOrderList(userVm.Id, paymentStatus);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetDemandOrderPaidList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderPaidList()
        {
            try
            {
                var userVm = await UserId();
                int paymentStatus = (int)PaymentStatusEnum.Paid;
                var demandOrderList = _salesSvc.GetDemandOrderList(userVm.Id, paymentStatus);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetDemandOrderEarlyPaymentList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderEarlyPaymentList()
        {
            try
            {
                var userVm = await UserId();
                var demandOrderList = _salesSvc.GetDemandOrderEarlyPaymentList(userVm.Id);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderEarlyPaymentPendingList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderEarlyPaymentPendingList()
        {
            try
            {
                var userVm = await UserId();
                var demandOrderList = _salesSvc.GetDemandOrderEarlyPaymentPendingList(userVm.Id);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderEarlyPaymentPaidList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderEarlyPaymentPaidList()
        {
            try
            {
                var userVm = await UserId();
                var demandOrderList = _salesSvc.GetDemandOrderEarlyPaymentPaidList(userVm.Id);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        //[Route("LastNDOs/{n}")]
        //[HttpGet]
        //public async Task<IHttpActionResult> LastNDOs(int n = 10)
        //{
        //    try
        //    {
        //        var userVm = await UserId();

        //        var demandOrderList = _salesSvc.GetLastNDemandOrder(n);
        //        return Ok(demandOrderList);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}
        [Route("GetDemandOrder/{doId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderById(int doId)
        {
            try
            {
                var userVm = await UserId();
                int companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var demandOrder = _salesSvc.GetDemandOrderById(userVm.Id, doId);
                return Ok(new { company, demandOrder });
            }
             catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveDemandOrderTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveDemandOrderTransaction(DemandOrderTransactionVm doTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                int companyId = userVm.CompanyId;
                doTransactionVm.CreatedBy = userVm.Id;
                doTransactionVm.CreatedOn = DateTime.Now;
                var newDemandOrder = await _salesSvc.SaveDemandOrderTransaction(userVm.Id, doTransactionVm);
                return Ok(newDemandOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveInvoiceTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveInvoiceTransaction(InvoiceTransactionVm invoiceTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                invoiceTransactionVm.CreatedBy = userVm.Id;
                invoiceTransactionVm.CreatedOn = DateTime.Now;
                var newInvoice = await _salesSvc.SaveInvoiceTransaction(userVm.Id, invoiceTransactionVm);
                return Ok(newInvoice);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetProductList")]
        [HttpGet]
        public IHttpActionResult GetProductList()
        {
            try
            {
                var productList = _salesSvc.GetProductList();
                return Ok(productList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveDemandOrder")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveDemandOrder(DemandOrderModel demandOrderModel)
        {
            try
            {
                var userVm = await UserId();
                demandOrderModel.CreatedBy = userVm.Id;
                demandOrderModel.CreatedOn = DateTime.Now;
                var newDemandOrder = _salesSvc.SaveDemandOrder(demandOrderModel);
                return Ok(newDemandOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateDemandOrder")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateDemandOrder(DemandOrderModel demandOrderModel)
        {
            try
            {
                var userVm = await UserId();
                demandOrderModel.UpdatedBy = userVm.Id;
                demandOrderModel.UpdatedOn = DateTime.Now;
                var newDemandOrder = _salesSvc.UpdateDemandOrder(demandOrderModel);
                return Ok(newDemandOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetPostOfficeList")]
        [HttpGet]
        public IHttpActionResult GetPostOfficeList()
        {
            try
            {
                var postOfficeList = _salesSvc.GetPostOfficeList();
                return Ok(postOfficeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetAreaList")]
        [HttpGet]
        public IHttpActionResult GetAreaList()
        {
            try
            {
                var areaList = _salesSvc.GetAreaList();
                return Ok(areaList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SubmitDO/{doId}")]
        [HttpPost]
        public async Task<IHttpActionResult> SubmitDo(int doId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.SubmitDO(doId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("VerifyDO/{doId}")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyDo(int doId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.VerifyDO(doId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApproveDO/{doId}")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveDo(int doId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.ApproveDO(doId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("DeliveryConfirmedDO/{doId}")]
        [HttpPost]
        public async Task<IHttpActionResult> DeliveredDo(int doId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.DeliveryConfirmedDO(doId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetInvoiceList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInvoiceList()
        {
            try
            {
                var userVm = await UserId();

                var invoiceList = _salesSvc.GetInvoiceList(userVm.Id);
                return Ok(invoiceList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetDemandOrderIdListForInvoice")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderIdListForInvoice()
        {
            try
            {
                var userVm = await UserId();

                var demandOrderListForInvoice = _salesSvc.GetDemandOrderIdListForInvoice(userVm.Id);
                return Ok(demandOrderListForInvoice);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetDemandOrderByIdFromInvoice/{doId}/{invoiceId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderByIdFromInvoice(int doId, int invoiceId)
        {
            try
            {
                var userVm = await UserId();

                var demandOrder = _salesSvc.GetDemandOrderByIdFromInvoice(userVm.Id, doId, invoiceId);
                return Ok(demandOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveInvoice")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveInvoice(InvoiceVm invoiceVm)
        {
            try
            {
                var userVm = await UserId();
                invoiceVm.CreatedBy = userVm.Id;
                invoiceVm.CreatedOn = DateTime.Now;
                var newInvoice = _salesSvc.SaveInvoice(invoiceVm);
                return Ok(newInvoice);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateInvoice")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateInvoice(InvoiceVm invoiceVm)
        {
            try
            {
                var userVm = await UserId();
                invoiceVm.UpdatedBy = userVm.Id;
                invoiceVm.UpdatedOn = DateTime.Now;
                var newInvoice = _salesSvc.UpdateInvoice(invoiceVm);
                return Ok(newInvoice);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetInvoiceById/{invoiceId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInvoiceById(int invoiceId)
        {
            try
            {
                var userVm = await UserId();
                var companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var invoice = _salesSvc.GetInvoiceById(userVm.Id, invoiceId);
                return Ok(new { company, invoice });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApproveInvoice")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveInvoice(InvoiceRequestVm invoiceRequestVm)
        {
            try
            {
                var userVm = await UserId();
                invoiceRequestVm.UserId = userVm.Id;
                invoiceRequestVm.CompanyId = userVm.CompanyId;
                invoiceRequestVm.DatedOn = DateTime.Now;
                await _salesSvc.ApproveInvoice(invoiceRequestVm);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("DeliveryInvoice/{invoiceId}")]
        [HttpPost]
        public async Task<IHttpActionResult> DeliveryInvoice(int invoiceId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.DeliveryInvoice(invoiceId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetCustomerTransactionHistoryByCustomerId/{doId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCustomerTransactionHistoryByCustomerId(int doId)
        {
            try
            {
                var userVm = await UserId();

                var customerTransactionHistory = _salesSvc.GetCustomerTransactionHistoryByCustomerId(userVm.Id, doId);
                return Ok(customerTransactionHistory);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetSalesPersonHistoryByEmployeeId/{employeeId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesPersonHistoryByEmployeeId(int employeeId)
        {
            try
            {
                var year = DateTime.Now.Year;
                var userVm = await UserId();
                var salesPersonHistory = _salesSvc.GetSalesPersonHistoryByEmployeeId(userVm.Id, employeeId, year);
                return Ok(salesPersonHistory);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("PayDOEarlyPaymentDiscountToCustomer/{doId}")]
        [HttpPost]
        public async Task<IHttpActionResult> PayDOEarlyPaymentDiscountToCustomer(int doId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.PayDOEarlyPaymentDiscountToCustomer(doId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderEarlyPaymentPendingTransactionList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderEarlyPaymentPendingTransactionList()
        {
            try
            {
                var userVm = await UserId();
                var demandOrderList = _salesSvc.GetDemandOrderEarlyPaymentPendingTransactionList(userVm.Id);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetDemandOrderEarlyPaymentApprovedTransactionList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDemandOrderEarlyPaymentApprovedTransactionList()
        {
            try
            {
                var userVm = await UserId();
                var demandOrderList = _salesSvc.GetDemandOrderEarlyPaymentApprovedTransactionList(userVm.Id);
                return Ok(demandOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("VerifyDOEarlyPaymentDiscountToCustomer")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyDOEarlyPaymentDiscountToCustomer(DemandOrderEarlyPaymentRequestVm demandOrderEarlyPaymentRequestVm)
        {
            try
            {
                var userVm = await UserId();
                demandOrderEarlyPaymentRequestVm.UserId = userVm.Id;
                demandOrderEarlyPaymentRequestVm.CompanyId = userVm.CompanyId;
                demandOrderEarlyPaymentRequestVm.DatedOn = DateTime.Now;
                await _salesSvc.VerifyDOEarlyPaymentDiscountToCustomer(demandOrderEarlyPaymentRequestVm);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetCompanySalesTargetList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompanySalesTargetList()
        {
            try
            {
                var userVm = await UserId();
                var salesTargetList = _salesSvc.GetCompanySalesTargetList(userVm.Id);
                return Ok(salesTargetList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveCompanySalesTarget")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveCompanySalesTarget(CompanySalesTargetVm companySalesTarget)
        {
            try
            {
                var userVm = await UserId();
                companySalesTarget.CreatedBy = userVm.Id;
                companySalesTarget.CreatedOn = DateTime.Now;
                var newSalesTarget = _salesSvc.SaveCompanySalesTarget(companySalesTarget);
                return Ok(newSalesTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("GetCompanySalesTargetById/{companySalesTargetId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetCompanySalesTargetById(int companySalesTargetId)
        {
            try
            {
                var userVm = await UserId();
                var companySalesTarget = _salesSvc.GetCompanySalesTargetById(userVm.Id, companySalesTargetId);
                return Ok(companySalesTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateCompanySalesTarget")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateCompanySalesTarget(CompanySalesTargetVm companySalesTarget)
        {
            try
            {
                var userVm = await UserId();
                companySalesTarget.UpdatedBy = userVm.Id;
                companySalesTarget.UpdatedOn = DateTime.Now;
                var newSalesTarget = _salesSvc.UpdateCompanySalesTarget(companySalesTarget);
                return Ok(newSalesTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("ApproveCompanySalesTarget/{companySalesTargetId}")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveCompanySalesTarget(int companySalesTargetId)
        {
            try
            {
                var userVm = await UserId();
                await _salesSvc.ApproveCompanySalesTarget(companySalesTargetId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesTeamTargetList/{year}/{month}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesTeamTargetList(int year, int month)
        {
            try
            {
                var userVm = await UserId();
                var salesTeamTargetList = _salesSvc.GetSalesTeamTargetList(userVm.Id, year, month);
                return Ok(salesTeamTargetList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveSalesTeamTarget")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveSalesTeamTarget(List<SalesTeamTargetVm> salesTeamTargetVmList)
        {
            try
            {
                var userVm = await UserId();
                var newSalesTeamTarget = _salesSvc.SaveSalesTeamTarget(salesTeamTargetVmList);
                return Ok(newSalesTeamTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("GetSalesTeamTargetById/{salesTeamTargetId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesTeamTargetById(int salesTeamTargetId)
        {
            try
            {
                var userVm = await UserId();
                var salesTeamTarget = _salesSvc.GetSalesTeamTargetById(userVm.Id, salesTeamTargetId);
                return Ok(salesTeamTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateSalesTeamTarget")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSalesTeamTarget(SalesTeamTargetVm salesTeamTargetVm)
        {
            try
            {
                var userVm = await UserId();
                salesTeamTargetVm.UpdatedBy = userVm.Id;
                salesTeamTargetVm.UpdatedOn = DateTime.Now;
                var newSalesTeamTarget = _salesSvc.UpdateSalesTeamTarget(salesTeamTargetVm);
                return Ok(newSalesTeamTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetSalesDivisionList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesDivisionList()
        {
            try
            {
                var userVm = await UserId();
                var salesDivisionList = _salesSvc.GetSalesDivisionList(userVm.Id);
                return Ok(salesDivisionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesAreaList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesAreaList()
        {
            try
            {
                var userVm = await UserId();
                var salesAreaList = _salesSvc.GetSalesAreaList(userVm.Id);
                return Ok(salesAreaList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesAreaWithSalesOfficerWithCustomerList/{salesDivisionId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesAreaWithSalesOfficerWithCustomerList(int salesDivisionId)
        {
            try
            {
                var vm = await UserId();
                var salesAreaList = await _salesSvc.GetSalesAreaBySalesDivisionId(salesDivisionId);
                var salesOfficerList = await _salesSvc.GetSalesOfficerBySalesDivisionId(salesDivisionId, vm.EmployeeId);
                var customerList = await _salesSvc.GetCustomerBySalesDivisionId(salesDivisionId);
                return Ok(new { SA = salesAreaList, SO = salesOfficerList, Customers = customerList });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesReportList/{startDate}/{endDate}/{salesDivisionId}/{salesAreaId}/{employeeId}/{customerId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSalesReportList(DateTime startDate, DateTime endDate, int salesDivisionId, int salesAreaId, int employeeId, int customerId)
        {
            try
            {
                var userVm = await UserId();
                int companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var salesReportList = _salesSvc.GetSalesReportList(startDate, endDate, salesDivisionId, salesAreaId, employeeId, customerId);
                return Ok(new { company, salesReportList });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetProductionForecastList/{year}/{month}")]
        [HttpGet]
        public IHttpActionResult GetProductionForecastList(int year, int month)
        {
            try
            {
                var productionForecastList = _salesSvc.GetProductionForecastList(year, month);
                return Ok(productionForecastList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveProductionForecast")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveProductionForecast(List<ProductionForecastVm> productionForecastList)
        {
            try
            {
                var userVm = await UserId();
                var newSalesTeamTarget = _salesSvc.SaveProductionForecast(productionForecastList);
                return Ok(newSalesTeamTarget);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        //invoice return process start

        [Route("InvoiceReturnList")]
        [HttpGet]
        public async Task<IHttpActionResult> InvoiceReturnList()
        {
            try
            {
                var userVm = await UserId();
                var returnInvoiceList = _salesSvc.InvoiceReturnList();
                return Ok(returnInvoiceList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetAllInvoiceList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllInvoiceList()
        {
            try
            {
                var userVm = await UserId();
                var allInvoiceList = _salesSvc.GetAllInvoiceList();
                return Ok(allInvoiceList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveReturnInvoice")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveReturnInvoice(InvoiceReturnVm invoiceReturnVm)
        {
            try
            {
                var userVm = await UserId();
                invoiceReturnVm.CreatedBy = userVm.Id;
                invoiceReturnVm.CreatedOn = DateTime.Now;
                var newInvoice = _salesSvc.SaveReturnInvoice(invoiceReturnVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetInvoiceReturnById/{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInvoiceReturnById(int Id)
        {
            try
            {
                var userVm = await UserId();
                var returnInvoice = _salesSvc.GetInvoiceReturnById(Id);
                return Ok(returnInvoice);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateReturnInvoice")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateReturnInvoice(InvoiceReturnVm invoiceReturnVm)
        {
            try
            {
                var userVm = await UserId();
                invoiceReturnVm.UpdatedBy = userVm.Id;
                invoiceReturnVm.UpdateOn = DateTime.Now;
                var newInvoice = _salesSvc.UpdateReturnInvoice(invoiceReturnVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApproveReturnInvoice/{Id}")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveReturnInvoice(int id)
        {
            try
            {
                InvoiceReturnVm invoiceReturnVm = new InvoiceReturnVm();
                var userVm = await UserId();
                invoiceReturnVm.ApprovedBy = userVm.Id;
                invoiceReturnVm.Id = id;
                invoiceReturnVm.CompanyId = userVm.CompanyId;
                invoiceReturnVm.ApprovedOn = DateTime.Now;
                var newInvoice = _salesSvc.ApproveReturnInvoice(invoiceReturnVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        //invoice return process end
        //Invoice Delivery Quantity/Challan Start
        [Route("InvoiceDeliveryQuantityList")]
        [HttpGet]
        public async Task<IHttpActionResult> InvoiceDeliveryQuantityList()
        {
            try
            {
                var userVm = await UserId();
                var deliveryChallanList = _salesSvc.InvoiceDeliveryChallanList();
                return Ok(deliveryChallanList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("InvoiceDetailsByIdForDeliveryChallan/{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> InvoiceDetailsByIdForDeliveryChallan(int Id)
        {
            try
            {
                var userVm = await UserId();
                var invoiceDetails = _salesSvc.InvoiceDetailsByIdForDeliveryChallan(Id);
                return Ok(invoiceDetails);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("DeliveryQuantitySave")]
        [HttpPost]
        public async Task<IHttpActionResult> DeliveryQuantitySave(DeliveryQuantityVm deliveryQuantityVm)
        {
            try
            {
                var userVm = await UserId();
                deliveryQuantityVm.CreatedBy = userVm.Id;
                deliveryQuantityVm.CreatedOn = DateTime.Now.Date;
                var deliveryAdd = _salesSvc.DeliveryQuantitySave(deliveryQuantityVm);
                return Ok(deliveryAdd.Id);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetDeliveryQuantityById/{Id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDeliveryQuantityById(int Id)
        {
            try
            {
                var userVm = await UserId();
                var companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var deliveryChallan = _salesSvc.GetDeliveryQuantityById(Id);
                return Ok(new { company, deliveryChallan });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("DeliveryQuantityUpdate")]
        [HttpPost]
        public async Task<IHttpActionResult> DeliveryQuantityUpdate(DeliveryQuantityVm deliveryQuantityVm)
        {
            try
            {
                var userVm = await UserId();
                deliveryQuantityVm.UpdateBy = userVm.Id;
                deliveryQuantityVm.UpdatedOn = DateTime.Now.Date;
                var deliveryEdit = _salesSvc.DeliveryQuantityUpdate(deliveryQuantityVm);
                return Ok(deliveryEdit);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApproveDeliveryQuantityById/{Id}")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveDeliveryQuantityById(int Id)
        {
            try
            {
                var userVm = await UserId();
                DeliveryQuantityVm dataVm = new DeliveryQuantityVm
                {
                    Id = Id,
                    ApprovedBy = userVm.Id,
                    ApprovedOn = DateTime.Now.Date
                };

                var deliveryApproved = _salesSvc.ApproveDeliveryQuantityById(dataVm);
                return Ok(deliveryApproved);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetUndeliveryQuantityById/{Id}")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUndeliveryQuantityById(int Id)
        {
            try
            {
                var userVm = await UserId();
                var companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var undeliveryChallan = _salesSvc.GetUndeliveryQuantityById(Id);
                return Ok(new { company, undeliveryChallan });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        //Invoice Delivery Quantity/Challan End

        //Total sales report list

        [Route("GetTotalSalesReportList/{StartDate}/{EndDate}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTotalSalesReportList(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var userVm = await UserId();
                var salesTotalReportList = _salesSvc.GetTotalSalesReportList(StartDate, EndDate);
                return Ok(new { salesTotalReportList});
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
