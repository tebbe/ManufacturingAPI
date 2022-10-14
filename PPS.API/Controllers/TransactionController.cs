using PPS.API.HelperClasses;
using PPS.Core;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PPS.API.Shared.ViewModel.Transaction;
using PPS.Shared.Core.HelperClasses;
using System.Threading.Tasks;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : BaseApiController
    {
        ITransactionInterface _transactionSvc;
        ILogger _logger;
        public TransactionController()
        {
            _transactionSvc = new TransactionService();
            _logger = new Logger();
        }
        [Route("GetTransactionAccountsPendingList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetTransactionAccountsPendingList(int fiscalYear, int companyId)
        {
            try
            {
                var tranList = _transactionSvc.GetUnapprovedAccountsTransaction(fiscalYear, companyId);
                return Ok(tranList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetTransactionSalesPendingList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetTransactionSalesPendingList(int fiscalYear, int companyId)
        {
            try
            {
                var tranList = _transactionSvc.GetUnapprovedSalesTransaction(fiscalYear, companyId);
                return Ok(tranList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetTransactionPurchasePendingList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetTransactionPurchasePendingList(int fiscalYear, int companyId)
        {
            try
            {
                var tranList = _transactionSvc.GetUnapprovedPurchaseTransaction(fiscalYear, companyId);
                return Ok(tranList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetTransactionRejectReasonType")]
        [HttpGet]
        public IHttpActionResult GetTransactionRejectReasonType()
        {
            try
            {
                var tranList = _transactionSvc.GetTransactionRejectReasonType();
                return Ok(tranList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetTransactionAccountsRejectedList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetTransactionAccountsRejectedList(int fiscalYear, int companyId)
        {
            try
            {
                var tranList = _transactionSvc.GetTransactionAccountsRejectedList(fiscalYear, companyId);
                return Ok(tranList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetTransactionByTransactionNo/{fiscalYear}/{companyId}/{transactionNo}")]
        [HttpGet]
        public IHttpActionResult GetTransactionByTransactionNo(int fiscalYear, int companyId, string transactionNo)
        {
            try
            {
                var tran = _transactionSvc.GetTransactionByTransactionNo(fiscalYear, companyId, transactionNo);
                return Ok(tran);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
