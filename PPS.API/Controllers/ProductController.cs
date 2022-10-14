using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel.Product;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : BaseApiController
    {
        private readonly IProductInterface _productInterface;
        ILogger _logger;
        public ProductController()
        {
            _productInterface = new ProductService();
            _logger = new Logger();
        }

        [Route("GetProductList")]
        [HttpGet]
        public IHttpActionResult GetProductList()
        {
            try
            {
                var poductList = _productInterface.GetProductList();
                return Ok(poductList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetProductById/{Id}")]
        [HttpGet]
        public IHttpActionResult GetProductById(int Id)
        {
            try
            {
                var productById = _productInterface.GetProductById(Id);
                return Ok(productById);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveProduct")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveProduct(ProductVm productVm)
        {
            try
            {
                var userVm = await UserId();
                productVm.CreatedBy = userVm.Id;
                productVm.CompanyId = userVm.CompanyId;
                productVm.CreatedOn = DateTime.Now;
                var addProduct = _productInterface.SaveProduct(productVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("UpdateProduct")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateProduct(ProductVm productVm)
        {
            try
            {
                var userVm = await UserId();
                productVm.UpdatedBy = userVm.Id;
                productVm.UpdatedOn = DateTime.Now.Date;
                productVm.CompanyId = userVm.CompanyId;
                var updateProduct = _productInterface.UpdateProduct(productVm);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetProductHistoryByProductId/{ProductId}")]
        [HttpGet]
        public IHttpActionResult GetProductHistoryByProductId(int ProductId)
        {
            try
            {
                var productHistoryById = _productInterface.GetProductHistoryByProductId(ProductId);
                return Ok(productHistoryById);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetProductRelatedAllDropdownList")]
        [HttpGet]
        public IHttpActionResult GetProductRelatedAllDropdownList()
        {
            try
            {
                var standardType = _productInterface.GetProductStandardTypeList();
                var unitType = _productInterface.GetUnitTypeList();
                var productType = _productInterface.GetProductTypeList();
                var productTypeGroup = _productInterface.GetAccountSubHeadCategory();
                return Ok(new { StandardType = standardType, UnitType = unitType, ProductType = productType,ProductTypeGroup= productTypeGroup });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        
        [Route("GetProductDeliveryReport/{startDate}/{endDate}/{customerId}/{productId}")]
        [HttpGet]
        public IHttpActionResult GetProductDeliveryReport(DateTime? startDate,DateTime? endDate,int? customerId, int? productId)
        {
            try
            {
                DatePickerVm datePickerVm = new DatePickerVm
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    CustomerId = customerId,
                    ProductId = productId
                };
                var productDeliveryReport = _productInterface.GetProductDeliveryReport(datePickerVm);
                return Ok(productDeliveryReport);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
