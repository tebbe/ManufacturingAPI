using PPS.API.HelperClasses;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.ViewModel.Store;
using PPS.Shared.Core.HelperClasses;

namespace PPS.API.Controllers
{
    //[RoutePrefix("api/Store")]
    public class BatchController : BaseApiController
    {
        readonly IBatchInterface _batchSvc;
        readonly ILogger _logger;
        public BatchController()
        {
            _batchSvc = new BatchService();
            _logger = new Logger();
        }
        
        [Route("GetBatchRequisitionList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBatchRequisitionList()
        {
            try
            {
                var userVm = await UserId();
                var batchRequisitionList = _batchSvc.GetBatchRequisitionList(userVm.Id);
                return Ok(batchRequisitionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetRawMaterialType")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRawMaterialType()
        {
            try
            {
                var userVm = await UserId();

                var rawMaterialType = _batchSvc.GetRawMaterialType(userVm.Id);
                return Ok(rawMaterialType);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveBatchRequisition")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveBatchRequisition(BatchRequisitionVm batchRequisitionModel)
        {
            try
            {
                var userVm = await UserId();
                batchRequisitionModel.CreatedBy = userVm.Id;
                batchRequisitionModel.CreatedOn = DateTime.Now;
                var newBatchRequisition = _batchSvc.SaveBatchRequisition(batchRequisitionModel);
                return Ok(newBatchRequisition);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
