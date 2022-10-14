using PPS.API.HelperClasses;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using PPS.API.Shared.RequestVm;
using PPS.API.Shared.ViewModel.Purchase;
using PPS.Shared.Core.HelperClasses;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Purchase")]
    public class PurchaseController : BaseApiController
    {
        readonly IPurchaseInterface _purchaseSvc;
        readonly ILogger _logger;
        public PurchaseController()
        {
            _purchaseSvc = new PurchaseService();
            _logger = new Logger();
        }
        
        [Route("GetPurchaseOrderList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPurchaseOrderList()
        {
            try
            {
                var userVm = await UserId();
                var purchaseOrderList = _purchaseSvc.GetPurchaseOrderList(userVm.Id);
                return Ok(purchaseOrderList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetSupplierList")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSupplierList()
        {
            try
            {
                var userVm = await UserId();
                var supplierList = _purchaseSvc.GetSupplierList(userVm.Id);
                return Ok(supplierList);
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
                var rawMaterialType = _purchaseSvc.GetRawMaterialType(userVm.Id);
                return Ok(rawMaterialType);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SavePurchaseOrder")]
        [HttpPost]
        public async Task<IHttpActionResult> SavePurchaseOrder(PurchaseOrderModel purchaseOrderModel)
        {
            try
            {
                var userVm = await UserId();
                purchaseOrderModel.CreatedBy = userVm.Id;
                purchaseOrderModel.CreatedOn = DateTime.Now;
                var newPurchaseOrder = _purchaseSvc.SavePurchaseOrder(purchaseOrderModel);
                return Ok(newPurchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        public async Task<IHttpActionResult> UpdatePurchaseOrder(PurchaseOrderModel purchaseOrderModel)
        {
            try
            {
                var userVm = await UserId();
                purchaseOrderModel.UpdatedBy = userVm.Id; 
                purchaseOrderModel.UpdatedOn = DateTime.Now;
                var newPurchaseOrder = _purchaseSvc.UpdatePurchaseOrder(purchaseOrderModel);
                return Ok(newPurchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetPurchaseOrderById/{poId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPurchaseOrderById(int poId)
        {
            try
            {
                var userVm = await UserId();

                var newPurchaseOrder = _purchaseSvc.GetPurchaseOrderById(userVm.Id, poId);
                return Ok(newPurchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("VerifyPO/{poId}")]
        [HttpPost]
        public async Task<IHttpActionResult> VerifyPo(int poId)
        {
            try
            {
                var userVm = await UserId();
                await _purchaseSvc.VerifyPO(poId, userVm.Id);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApprovePO")]
        [HttpPost]
        public async Task<IHttpActionResult> ApprovePo(PurchaseOrderWithSupplierRequestVm purchaseOrderWithSupplierRequestVm)
        {
            try
            {
                var userVm = await UserId();
                purchaseOrderWithSupplierRequestVm.UserId = userVm.Id;
                purchaseOrderWithSupplierRequestVm.CompanyId = userVm.CompanyId;
                purchaseOrderWithSupplierRequestVm.DatedOn = DateTime.Now;
                var approvePurchaseOrder = _purchaseSvc.ApprovePO(purchaseOrderWithSupplierRequestVm);

                if (approvePurchaseOrder.Success == false)
                {
                    _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, approvePurchaseOrder.Message);
                }
                return Ok(approvePurchaseOrder);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetSupplierById/{supplierId}/{purchaseOrderId}")]
        [HttpGet]
        public IHttpActionResult GetSupplierById(int supplierId, int purchaseOrderId)
        {
            try
            {
                var requestVm = new PurchaseOrderWithSupplierRequestVm
                {
                    SupplierId = supplierId,
                    PurchaseOrderId = purchaseOrderId
                };
                
                var supplierVm = _purchaseSvc.GetSupplierById(requestVm);
                return Ok(supplierVm);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("SavePurchaseOrderTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> SavePurchaseOrderTransaction(PurchaseOrderTransactionVm purchaseOrderTransactionVm)
        {
            try
            {
                var userId = await UserId();
                purchaseOrderTransactionVm.CreatedBy = userId.Id;
                purchaseOrderTransactionVm.CreatedOn = DateTime.Now;
                var createdPurchaseOrderTransaction = await _purchaseSvc.SavePurchaseOrderTransaction(userId.Id, purchaseOrderTransactionVm);
                return Ok(createdPurchaseOrderTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("ApprovePurchaseOrderTransaction")]
        [HttpPost]
        public async Task<IHttpActionResult> ApprovePurchaseOrderTransaction(PurchaseOrderTransactionVm purchaseOrderTransactionVm, int fiscalYear)
        {
            try
            {
                var userVm = await UserId();
                purchaseOrderTransactionVm.ApprovedBy = userVm.Id;
                purchaseOrderTransactionVm.ApprovedOn = DateTime.Now;
                var approvePurchaseOrderTransaction = _purchaseSvc.ApprovePurchaseOrderTransaction(userVm, purchaseOrderTransactionVm, fiscalYear);
                if (approvePurchaseOrderTransaction.Success == false)
                {
                    _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, approvePurchaseOrderTransaction.Message);
                }
                return Ok(approvePurchaseOrderTransaction);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetUnapprovedPurchaseOrderTransaction")]
        [HttpGet]
        public IHttpActionResult GetUnapprovedPurchaseOrderTransaction()
        {
            try
            {
                var purchaseOrderVm = _purchaseSvc.GetUnapprovedPurchaseOrderTransaction();
                return Ok(purchaseOrderVm);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetApprovedPurchaseOrderTransaction")]
        [HttpGet]
        public IHttpActionResult GetApprovedPurchaseOrderTransaction()
        {
            try
            {
                var purchaseOrderVm = _purchaseSvc.GetApprovedPurchaseOrderTransaction();
                return Ok(purchaseOrderVm);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
