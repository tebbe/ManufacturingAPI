using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel.Product;
using PPS.API.Shared.ViewModel.Report;
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
    [RoutePrefix("api/Report")]
    public class ReportController : BaseApiController
    {
        IReportService _reportService;
        ILogger _logger;
        readonly ICompanyService _companyService;
        public ReportController()
        {
            _reportService = new ReportService();
            _logger = new Logger();
            _companyService = new CompanyService();
        }

        [Route("GetLedger/{fiscalYear}/{companyId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetLedger(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var ledgerList = _reportService.GetLedger(fiscalYear, companyId, startDate, endDate);

                return Ok(ledgerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetJournal/{fiscalYear}/{companyId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetJournal(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var journalList = _reportService.GetJournal(fiscalYear, companyId, startDate, endDate);

                return Ok(journalList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetIndividualLedger/{fiscalYear}/{companyId}/{headId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetIndividualLedger(int fiscalYear, int companyId, int headId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var ledgerList = _reportService.GetIndividualLedger(fiscalYear, companyId, headId, startDate, endDate);

                return Ok(ledgerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetTrailBalance/{fiscalYear}/{companyId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetTrailBalance(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var ledgerList = _reportService.GetTrailBalance(fiscalYear, companyId, startDate, endDate);

                return Ok(ledgerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetProfitAndLossAccount/{fiscalYear}/{companyId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetProfitAndLossAccount(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var ledgerList = _reportService.GetProfitAndLossAccount(fiscalYear, companyId, startDate, endDate);

                return Ok(ledgerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetBalanceSheet/{fiscalYear}/{companyId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetBalanceSheet(int fiscalYear, int companyId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var ledgerList = _reportService.GetBalanceSheet(fiscalYear, companyId, startDate, endDate);

                return Ok(ledgerList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetCustomerStatement/{companyId}/{customerId}/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetCustomerStatement(int companyId, int customerId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var company = _companyService.GetCompanyById(companyId);
                var customerStatement = _reportService.GetCustomerStatement(companyId, customerId, startDate, endDate);

                return Ok(new { company, customerStatement });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetVoucherDetail/{tranNo}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetVoucherDetail(string tranNo)
        {
            try
            {
                var userVm = await UserId();
                int companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var voucherDetail = _reportService.GetVoucherDetail(tranNo);
                return Ok(new { company, voucherDetail });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetTransactionHistoryByTransactionNo/{tranNo}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactionHistoryByTransactionNo(string tranNo)
        {
            try
            {
                var userVm = await UserId();
                int companyId = userVm.CompanyId;
                var company = _companyService.GetCompanyById(companyId);
                var history = _reportService.GetTransactionHistoryByTransactionNo(companyId, tranNo);
                return Ok(new { company, history });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetProductReportList/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult GetProductReportList(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                DatePickerVm viewModel = new DatePickerVm
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
                var deliveryReport = _reportService.GetProductReportList(viewModel);
                return Ok(deliveryReport);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("DealerAuditReport/{startDate}/{endDate}")]
        [HttpGet]
        public IHttpActionResult DealerAuditReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                DatePickerVm dataVm = new DatePickerVm
                {
                    StartDate=startDate,
                    EndDate=endDate
                };
                var dealerAuditReport = _reportService.DealerAuditReport(dataVm);
                return Ok(dealerAuditReport);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
