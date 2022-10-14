using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PPS.API.Shared.Extensions;
using PPS.API.Shared.ViewModel.Account;
using System.Threading.Tasks;
using PPS.API.Shared.Enums;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Approval")]
    public class ApprovalController : BaseApiController
    {
        private IApprovalService _approvalSvc;
        ILogger _logger;

        public ApprovalController()
        {
            _approvalSvc = new ApprovalService();
            _logger = new Logger();
        }

        [Route("TransactionVerifyAccounts")]
        [HttpPost]
        public async Task<IHttpActionResult> TransactionVerifyAccounts(AcceptRejectTransactionVm verifyTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                verifyTransactionVm.UserId = userVm.Id;
                verifyTransactionVm.DatedOn = DateTime.Now;
                _approvalSvc.VerifyTransaction(verifyTransactionVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Your request has failed."));
            }
        }

        [Route("TransactionApproveAccounts")]
        [HttpPost]
        public async Task<IHttpActionResult> TransactionApproveAccounts(AcceptRejectTransactionVm acceptRejectTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                acceptRejectTransactionVm.UserId = userVm.Id;
                acceptRejectTransactionVm.DatedOn = DateTime.Now;
                _approvalSvc.AcceptTransaction(acceptRejectTransactionVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Your request has failed."));
            }
        }

        [Route("TransactionRejectAccounts")]
        [HttpPost]
        public async Task<IHttpActionResult> TransactionRejectAccounts(AcceptRejectTransactionVm acceptRejectTransactionVm)
        {
            try
            {
                var userVm = await UserId();
                acceptRejectTransactionVm.UserId = userVm.Id;
                acceptRejectTransactionVm.DatedOn = DateTime.Now;
                _approvalSvc.RejectTransaction(acceptRejectTransactionVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Your request has failed."));
            }
        }
    }
}
