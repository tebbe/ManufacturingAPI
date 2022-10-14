using PPS.API.HelperClasses;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using PPS.API.Shared.ViewModel.Document;
using System.Threading.Tasks;
using PPS.Data.Edmx;

namespace PPS.API.Controllers
{
    
    [RoutePrefix("api/Document")]
    public class DocumentController : BaseApiController
    {
        private readonly ILegalDocumentInterface _legalDocSvc;
        private readonly ILogger _logger;
        public DocumentController()
        {
            _legalDocSvc = new LegalDocumentService();
            _logger = new Logger();
        }
        [Route("GetLegalDocumentList")]
        [HttpGet]
        public IHttpActionResult GetLegalDocumentList()
        {
            try
            {
                var documentList = _legalDocSvc.GetLegalDocumentList();
                return Ok(documentList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetLegalDocumentType")]
        [HttpGet]
        public IHttpActionResult GetLegalDocumentType()
        {
            try
            {
                var docTypeList = _legalDocSvc.GetLegalDocumentType();
                return Ok(docTypeList);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetRenewalCategory")]
        [HttpGet]
        public IHttpActionResult GetRenewalCategory()
        {
            try
            {
                var docRenewalCategory = _legalDocSvc.GetRenewalCategory();
                return Ok(docRenewalCategory);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetLeglDocumentStatus")]
        [HttpGet]
        public IHttpActionResult GetLeglDocumentStatus()
        {
            try
            {
                var docStatus = _legalDocSvc.GetLeglDocumentStatus();
                return Ok(docStatus);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("SaveLegalDocument")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveLegalDocument(LegalDocumentVm legalDocument)
        {
            try
            {
                var userVm = await UserId();
                legalDocument.CreatedBy =Convert.ToString(userVm.Id);
                legalDocument.CreatedOn = DateTime.Now;
                var newLegalDocument = _legalDocSvc.SaveLegalDocument(legalDocument);
                return Ok(newLegalDocument);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("GetLegalDocumentById/{lDocId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLegalDocumentById(int lDocId)
        {
            try
            {
                var userVm = await UserId();
                var legaldocument = _legalDocSvc.GetLegalDocument(lDocId);
                return Ok(legaldocument);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("UpdateLegalDocument")]
        [HttpPost]
        public async Task<IHttpActionResult>UpdateLegalDocument(LegalDocumentVm legalDocumentVm)
        {
            try
            {
                var userVm = await UserId();
                legalDocumentVm.UpdatedBy = Convert.ToString(userVm.Id);
                legalDocumentVm.UpdatedOn = DateTime.Now;
                var newLegaldocument = _legalDocSvc.UpdateLegalDocument(legalDocumentVm);
                return Ok(newLegaldocument);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("LegalDocumentListPrint")]
        [HttpGet]
        public IHttpActionResult LegalDocumentListPrint()
        {
            try
            {
                var legalDocListPrint = _legalDocSvc.LegalDocumentListPrint();
                return Ok(legalDocListPrint);
            }
            catch (Exception ex)
            {

                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));

            }
        }

        [Route("LegalDocumentSinglePrint/{Id}")]
        [HttpGet]
        public IHttpActionResult LegalDocumentSinglePrint(int Id)
        {
            try
            {
                var legalDocSinglePrint = _legalDocSvc.LegalDocumentSinglePrint(Id);
                return Ok(legalDocSinglePrint);
            }
            catch (Exception ex)
            {

                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));

            }
        }
        [Route("LegalDocumentHistoryList/{Id}")]
        [HttpGet]
        public IHttpActionResult LegalDocumentHistoryList(int Id)
        {
            try
            {
                var document = _legalDocSvc.GetLegalDocById(Id);
                var history = _legalDocSvc.LegalDocumentHistory(Id);
                return Ok(new { doc = document, docHistory = history });
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
           
        }
    }
}