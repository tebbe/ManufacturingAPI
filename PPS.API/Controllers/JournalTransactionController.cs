using PPS.API.HelperClasses;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PPS.API.Shared.ViewModel.Transaction;
using PPS.Shared.Core.HelperClasses;
using System.Threading.Tasks;
using PPS.API.Shared.Enums;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/JournalTransaction")]
    public class JournalTransactionController : BaseApiController
    {
        readonly ITransactionInterface _transactionSvc;
        readonly ILogger _logger;
        public JournalTransactionController()
        {
            _transactionSvc = new TransactionService();
            _logger = new Logger();
        }

        [Route("GetJournalTransactionList/{fiscalYear}/{companyId}/{tranTypeId}")]
        [HttpGet]
        public IHttpActionResult GetJournalTransactionList(int fiscalYear, int companyId, int tranTypeId)
        {
            try
            {
                var tranList = _transactionSvc.GetTransactionList(fiscalYear, companyId, tranTypeId);
                return Ok(tranList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveJournalTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveTransaction(TransactionModel transactionModel)
        {
            try
            {
                var userVm = await UserId();
                transactionModel.PostingDate = DateTime.Now;
                transactionModel.CreatedDate = DateTime.Now;
                transactionModel.CreatedById = userVm.Id;
                transactionModel.UpdatedById = null;
                transactionModel.UpdatedDate = null;
                transactionModel.Accepted = false;
                transactionModel.AcceptedById = null;
                transactionModel.AcceptedDate = null;
                transactionModel.TranTypeId = (int) TransactionTypeEnum.Journal;
                var newTransaction = _transactionSvc.SaveTransaction(transactionModel);
                return Ok(newTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateJournalTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTransaction(TransactionModel transactionModel)
        {
            try
            {
                var userVm = await UserId();
                transactionModel.UpdatedDate = DateTime.Now;
                transactionModel.UpdatedById = userVm.Id;

                transactionModel.TranTypeId = (int) TransactionTypeEnum.Journal;
                var newTransaction = _transactionSvc.UpdateTransaction(transactionModel);
                return Ok(newTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
