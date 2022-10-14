using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel.Account;
using PPS.Core;
using PPS.Data.Dtos;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.ViewModel.User;
using PPS.Shared.Core.HelperClasses;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Ledger")]
    public class AccountsController : BaseApiController
    {
        ILedgerInterface _ledgerSvc;
        ILogger _logger;
        public AccountsController()
        {
            _ledgerSvc = new LedgerService();
            _logger = new Logger();
        }

        [Route("LedgerList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public ActionStatus LedgerList(int fiscalYear, int companyId)
        {
            try
            {
                var ledgerList = _ledgerSvc.GetLegerList(fiscalYear, companyId);
                
                var result = new ActionStatus(true);
                result.ResponseData = ledgerList;
                return result;
            }
            catch(Exception ex)
            {
                var error = new ActionStatus(false);
                error.AddMessage(ex.Message);
                return error;
            }
        }

        [Route("LedgerAccountTypeList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public ActionStatus LedgerAccountTypeList(int fiscalYear, int companyId)
        {
            try
            {
                var ledgerList = _ledgerSvc.GetAccountTypeList(fiscalYear, companyId);
                var result = new ActionStatus(true);
                result.ResponseData = ledgerList;
                return result;
            }
            catch (Exception ex)
            {
                var error = new ActionStatus(false);
                error.AddMessage(ex.Message);
                return error;
            }
        }

        [Route("GetAccountPrimaryHeadListForLedger/{fiscalYear}/{companyId}")]
        [HttpGet]
        public ActionStatus GetAccountPrimaryHeadListForLedger(int fiscalYear, int companyId)
        {
            try
            {
                var ledgerList = _ledgerSvc.GetAccountPrimaryHeadListForLedger(fiscalYear, companyId);
                var result = new ActionStatus(true);
                result.ResponseData = ledgerList;
                return result;
            }
            catch (Exception ex)
            {
                var error = new ActionStatus(false);
                error.AddMessage(ex.Message);
                return error;
            }
        }

        [Route("LedgerAccountPrimaryHeadList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public ActionStatus LedgerAccountPrimaryHeadList(int fiscalYear, int companyId)
        {
            try
            {
                var ledgerList = _ledgerSvc.GetAccountPrimaryHeadList(fiscalYear, companyId);
                var result = new ActionStatus(true);
                result.ResponseData = ledgerList;
                return result;
            }
            catch (Exception ex)
            {
                var error = new ActionStatus(false);
                error.AddMessage(ex.Message);
                return error;
            }
        }

        [Route("GetAccountHeadList/{fiscalYear}/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetAccountHeadList(int fiscalYear, int companyId)
        {
            try
            {
                var ledgerList = _ledgerSvc.GetAccountHeadList(fiscalYear, companyId);
                //var result = new ActionStatus(true);
                ///result.ResponseData = ledgerList;
                return Ok(ledgerList);
            }
            //catch (Exception ex)
            //{
            //    var error = new ActionStatus(false);
            //    error.AddMessage(ex.Message);
            //    return error;
            //}
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetAccountSubHeadList/{companyId}/{accountPrimaryHeadId}")]
        [HttpGet]
        public ActionStatus GetAccountSubHeadList(int companyId, int accountPrimaryHeadId)
        {
            try
            {
                var subHeadList = _ledgerSvc.GetAccountSubHeadList(companyId, accountPrimaryHeadId);
                var result = new ActionStatus(true);
                result.ResponseData = subHeadList;
                return result;
            }
            catch (Exception ex)
            {
                var error = new ActionStatus(false);
                error.AddMessage(ex.Message);
                return error;
            }
        }
        [Route("SaveAccountHead")]
        [HttpPost]
        public async Task<ActionStatus> SaveAccountHead(AccountHeadModel accountHead)
        {
            try
            {
                var userVm = new UserVm();
                accountHead.CreatedById = userVm.Id;
                accountHead.CreatedDate = DateTime.Now;
                accountHead.UpdatedById = userVm.Id;
                accountHead.UpdatedDate = DateTime.Now;
                var createdLedger = _ledgerSvc.SaveAccountHead(accountHead);
                
                var result = new ActionStatus(true);
                result.ResponseData = createdLedger;
                return result;
            }
            catch (Exception ex)
            {
                var error = new ActionStatus(false);
                error.AddMessage(ex.Message);
                return error;
            }
        }

        [Route("GetBankCashAccountHeadList/{customerId}")]
        [HttpGet]
        public IHttpActionResult GetBankCashAccountHeadList(int customerId)
        {
            try
            {
                var ledgerList = _ledgerSvc.GetBankCashAccountHeadList(customerId);
                return Ok(ledgerList);
            }            
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetBankCashAccountHeadList")]
        [HttpGet]
        public IHttpActionResult GetBankCashAccountHeadList()
        {
            try
            {
                var bankCashAccuntHeadList = _ledgerSvc.GetBankCashAccountHeadList();
                return Ok(bankCashAccuntHeadList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSalesAccount")]
        [HttpGet]
        public IHttpActionResult GetSalesAccount()
        {
            try
            {
                var salesEmployeeList = _ledgerSvc.GetSalesAccount();
                return Ok(salesEmployeeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetLCAccountHeadList")]
        [HttpGet]
        public IHttpActionResult GetLCAccountHeadList()
        {
            try
            {
                var lcAccuntHeadList = _ledgerSvc.GetLCAccountHeadList();
                return Ok(lcAccuntHeadList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
