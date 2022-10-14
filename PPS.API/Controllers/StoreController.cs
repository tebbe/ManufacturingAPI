using PPS.API.HelperClasses;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.ViewModel.Store;
using PPS.Shared.Core.HelperClasses;
using PPS.API.Shared.ViewModel.Filter;
using PPS.API.Shared.Enums;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Store")]
    public class StoreController : BaseApiController
    {
        readonly IStoreInterface _storeSvc;
        readonly ILogger _logger;
        public StoreController()
        {
            _storeSvc = new StoreService();
            _logger = new Logger();
        }

        [Route("GetBatchRequisitionList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBatchRequisitionList()
        {
            try
            {
                var userVm = await UserId();
                var batchRequisitionList = _storeSvc.GetBatchRequisitionList(userVm.Id);
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

                var rawMaterialType = _storeSvc.GetRawMaterialType(userVm.Id);
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
                var newBatchRequisition = _storeSvc.SaveBatchRequisition(batchRequisitionModel);
                return Ok(newBatchRequisition);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetPendingPOList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingPoList()
        {
            try
            {
                var userVm = await UserId();
                var pendingPoList = _storeSvc.GetPendingPOList(userVm.Id);
                return Ok(pendingPoList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetAcceptedPOList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAcceptedPoList()
        {
            try
            {
                var userVm = await UserId();
                var acceptedPoList = _storeSvc.GetAcceptedPOList(userVm.Id);
                return Ok(acceptedPoList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetPendingPOById/{poId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPendingPoById(int poId)
        {
            try
            {
                var userVm = await UserId();

                var purchaseOrder = _storeSvc.GetPendingPOById(userVm.Id, poId);
                return Ok(purchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveAcceptedPurchaseOrder")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveAcceptedPurchaseOrder(List<StoreRawMaterialVm> storeRawMaterialVm)
        {
            try
            {
                var userVm = await UserId();
                storeRawMaterialVm.ToList().ForEach(x=>x.ReceivedBy = userVm.Id);
                storeRawMaterialVm.ToList().ForEach(x=>x.ReceivedOn = DateTime.Now);
                var newstoreRawMaterial = _storeSvc.SaveAcceptedPurchaseOrder(storeRawMaterialVm);
                return Ok(newstoreRawMaterial);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetBatchRequisitionById/{brId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetBatchRequisitionById(int brId)
        {
            try
            {
                var userVm = await UserId();
                var batchRequisition = _storeSvc.GetBatchRequisitionById(userVm.Id, brId);
                return Ok(batchRequisition);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("DeliveryBR/{Id}")]
        [HttpPost]
        public async Task<IHttpActionResult> DeliveryBr(int id)
        {
            try
            {
                var userVm = await UserId();
                await _storeSvc.DeliveryBR(userVm.Id, id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ReceiveBR/{Id}")]
        [HttpPost]
        public async Task<IHttpActionResult> ReceiveBr(int id)
        {
            try
            {
                var userVm = await UserId();
                await _storeSvc.ReceiveBR(userVm.Id, id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SendToProductionBR")]
        [HttpPost]
        public async Task<IHttpActionResult> SendToProductionBr(BatchRequisitionVm batchRequisitionVm)
        {
            try
            {
                var userVm = await UserId();
                await _storeSvc.SendToProductionBR(userVm.Id, batchRequisitionVm);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        //[Route("GetBatchRequisitionListFromFloorStore")]
        //[HttpGet]
        //public async Task<IHttpActionResult> GetBatchRequisitionListFromFloorStore()
        //{
        //    try
        //    {
        //        var userVm = await UserId();
        //        var batchRequisitionList = _storeSvc.GetBatchRequisitionListFromFloorStore(userVm.Id);
        //        return Ok(batchRequisitionList);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }
        //}

        [Route("GetProductionGroupListFromFloorStore")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductionGroupListFromFloorStore()
        {
            try
            {
                var userVm = await UserId();
                var productionGroupList = _storeSvc.GetProductionGroupListFromFloorStore(userVm.Id);
                return Ok(productionGroupList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveFinishedGood")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveFinishedGood(List<FinishedGoodVm> finishedGoods, bool isClosedBatch)
        {
            try
            {
                var userVm = await UserId();
                finishedGoods.ToList().ForEach(x => x.CreatedBy = userVm.Id);
                finishedGoods.ToList().ForEach(x => x.CreatedOn = DateTime.Now);
                var newFinishedGood = _storeSvc.SaveFinishedGood(finishedGoods, isClosedBatch);
                return Ok(newFinishedGood);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetFinishedGood")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFinishedGood()
        {
            try
            {
                var userVm = await UserId();
                var batchRequisitionList = _storeSvc.GetFinishedGood(userVm.Id);
                return Ok(batchRequisitionList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetFinishedGoodForFiltering")]
        [HttpGet]
        public async Task<IHttpActionResult> GetFinishedGoodForFiltering(int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            try
            {
                var filterVm = new FilterVm
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SortColumn = sortColumn,
                    SortDirection = SortDirectionEnum.ASC.ToString() == sortDirection ? SortDirectionEnum.ASC : SortDirectionEnum.DESC
                };
                var userVm = await UserId();
                var finishedGoodList = _storeSvc.GetFinishedGoodForFiltering(userVm.Id, filterVm);
                return Ok(finishedGoodList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApproveFinishedGood/{pgId}")]
        [HttpPost]
        public async Task<IHttpActionResult> ApproveFinishedGood(int pgId)
        {
            try
            {
                var userVm = await UserId();
                await _storeSvc.ApproveFinishedGood(userVm.Id, pgId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("CloseBatch/{brId}")]
        [HttpPost]
        public async Task<IHttpActionResult> CloseBatch(int brId)
        {
            try
            {
                var userVm = await UserId();
                await _storeSvc.CloseBatch(userVm.Id, brId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveBRProductEstimation")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveBrProductEstimation(List<BatchRequisitionProductionEstimationVm> bRProductionEstimation)
        {
            try
            {
                await UserId();
                var newBrProductionEstimation = _storeSvc.SaveBRProductEstimation(bRProductionEstimation);
                return Ok(newBrProductionEstimation);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetProductionGroupList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProductionGroupList()
        {
            try
            {
                var userVm = await UserId();
                var productionGroupList = _storeSvc.GetProductionGroupList(userVm.Id);
                return Ok(productionGroupList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SaveProductionGroup")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveProductionGroup()
        {
            try
            {
                var userVm = await UserId();
                var productionGroupModel = new ProductionGroupVm
                {
                    CreatedBy = userVm.Id,
                    CreatedOn = DateTime.Now
                };
                var newProductionGroup = _storeSvc.SaveProductionGroup(productionGroupModel);
                return Ok(newProductionGroup);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("CloseProductionGroup/{pgId}")]
        [HttpPost]
        public async Task<IHttpActionResult> CloseProductionGroup(int pgId)
        {
            try
            {
                var userVm = await UserId();
                await _storeSvc.CloseProductionGroup(userVm.Id, pgId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
