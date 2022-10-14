﻿using PPS.API.HelperClasses;
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
    [RoutePrefix("api/PaymentTransaction")]
    public class PaymentTransactionController : BaseApiController
    {
        readonly ITransactionInterface _transactionSvc;
        readonly ILogger _logger;
        public PaymentTransactionController()
        {
            _transactionSvc = new TransactionService();
            _logger = new Logger();
        }

        [Route("GetPaymentTransactionList/{fiscalYear}/{companyId}/{tranTypeId}")]
        [HttpGet]
        public IHttpActionResult GetPaymentTransactionList(int fiscalYear, int companyId, int tranTypeId)
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
       
        [Route("SavePaymentTransaction")]
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
                transactionModel.UpdatedDate = (DateTime?)null;
                transactionModel.Accepted = false;
                transactionModel.AcceptedById = null;
                transactionModel.AcceptedDate = null;
                transactionModel.TranTypeId = (int) TransactionTypeEnum.Payment;
                var newTransaction = _transactionSvc.SaveTransaction(transactionModel);
                return Ok(newTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdatePaymentTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTransaction(TransactionModel transactionModel)
        {
            try
            {
                var userVm = await UserId();
                transactionModel.UpdatedDate = DateTime.Now;
                transactionModel.UpdatedById = userVm.Id;

                transactionModel.TranTypeId = (int) TransactionTypeEnum.Payment;
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
