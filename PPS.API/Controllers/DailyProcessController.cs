using PPS.API.HelperClasses;
using PPS.Operations.Service;
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
    [RoutePrefix("InternalApi/DailyProcess")]
    public class DailyProcessController : BaseInternalApiController
    {
        private ISystemWarningService _systemWarningService;
        private readonly ILogger _logger;
        public DailyProcessController()
        {
            _systemWarningService = new SystemWarningService();
        }
        [Route("CheckSystemWarning/{key}/{fiscalYear}/{companyId}/{userId}")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckSystemWarning(string key, int fiscalYear, int companyId, int userId)
        {
            try
            {
                var isSuccess = _systemWarningService.CheckSystemWarning(fiscalYear, companyId, userId);
                return Ok(isSuccess);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
