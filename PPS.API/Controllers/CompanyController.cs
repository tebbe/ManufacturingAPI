using PPS.API.HelperClasses;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/Company")]
    public class CompanyController : BaseApiController
    {
        private ICompanyService _companyService;
        PPS.Shared.Core.HelperClasses.ILogger _logger;

        public CompanyController()
        {
            _companyService = new CompanyService();
            _logger = new Logger();
        }
        
        [Route("GetCompanyList")]
        [HttpGet]
        public IHttpActionResult GetCompanyList()
        {
            try
            {
                var companies = _companyService.GetCompanyList();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetCompanyById/{companyId}")]
        [HttpGet]
        public IHttpActionResult GetCompanyById(int companyId)
        {
            try
            {
                var company = _companyService.GetCompanyById(companyId);
                return Ok(company);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
