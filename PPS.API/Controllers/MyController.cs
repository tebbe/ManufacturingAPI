using PPS.API.HelperClasses;
using PPS.API.Shared.ViewModel;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PPS.API.Shared.Extensions;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/My")]
    public class MyController : BaseApiController
    {
        private IUserService _userSvc;
        ILogger _logger;

        public MyController()
        {
            _userSvc = new UserService();
            _logger = new Logger();
        }

        [Route("UpdatePassword")]
        [HttpPost]
        public IHttpActionResult UpdatePassword(UserPasswordChangeModel userPasswordChangeModel)
        {
            try
            {
                if((userPasswordChangeModel.CurrentPassword.IsNullOrWhiteSpaceOrEmpty()))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Invalid current password."));
                }
                if(userPasswordChangeModel.NewPassword != userPasswordChangeModel.ConfirmPassword)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "New password and confirm password should match."));
                }
                if(userPasswordChangeModel.NewPassword.Length < 8)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Password should be at least 8 characters long."));
                }
                var user = _userSvc.UpdatePassword(userPasswordChangeModel);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Your request has failed."));
            }
        }
    }
}
