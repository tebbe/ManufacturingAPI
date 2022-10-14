using PPS.API.HelperClasses;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.Operation.Operation.Service;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/MonthlyProcess")]
    public class MonthlyProcessController : BaseApiController
    {
        private IMonthlySalesProcess monthlySalesProcess;
        ILogger logger;

        public MonthlyProcessController()
        {
            monthlySalesProcess = new MonthlySalesProcess();
            logger = new Logger();
        }

        [Route("GetMonthlySalesProcess")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMonthlySalesProcess()
        {
            try
            {
                var salesProcess = await monthlySalesProcess.GetMonthlySalesProcess();
                return Ok(salesProcess);
            }
            catch (Exception ex)
            {
                logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveMonthlySalesProcessing/{year}/{month}")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveMonthlySalesProcessing(int year, int month)
        {
            try
            {
                var userVm = await UserId();
                var reprocess = false;
                var hasProcessed = await monthlySalesProcess.ProcessMonthlyAchievement(month, year, userVm.Id, reprocess);
                return Ok(hasProcessed);
            }
            catch (Exception ex)
            {
                logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ReprocessMonthlySalesProcessing/{year}/{month}")]
        [HttpPost]
        public async Task<IHttpActionResult> ReprocessMonthlySalesProcessing(int year, int month)
        {
            try
            {
                var userVm = await UserId();
                var reprocess = true;
                var hasProcessed = await monthlySalesProcess.ProcessMonthlyAchievement(month, year, userVm.Id, reprocess);
                return Ok(hasProcessed);
            }
            catch (Exception ex)
            {
                logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
