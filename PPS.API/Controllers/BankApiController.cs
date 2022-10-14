using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Bank")]
    public class BankApiController : ApiController
    {
        IBankService _bankSvc;
        ILogger _logger;
        public BankApiController()
        {
            _bankSvc = new BankService();
            _logger = new Logger();
        }
        [Route("GetBankList")]
        [HttpGet]
        public IHttpActionResult GetBankList()
        {
            try
            {
                var bankList = _bankSvc.GetBankList();
                return Ok(bankList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
