using Microsoft.AspNet.Identity;
using PPS.API.Shared.ViewModel.User;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace PPS.API.HelperClasses
{
    public class BaseApiController : ApiController
    {
        private readonly IUserService _userService;
        public BaseApiController()
        {
            _userService = new UserService();
        }
        protected async Task<UserVm> UserId()
        {
            var aspNetUserId = User.Identity.GetUserId();
            var user = await _userService.GetUserByAspNetUserId(aspNetUserId);

            if (user == null)
            {
                return null;
            }

            var userVm = new UserVm
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId ?? -1,
                CompanyId = user.CompanyId,
                AspNetUserId = aspNetUserId
            };
            return userVm;
        }
        protected ActionStatus ValidateUser(string username, string password)
        {
            try
            {
                //TODO: Validate User 

                return new ActionStatus(true);
            } 
            catch(Exception ex)
            {
                // TODO: Log
                return new ActionStatus(ex);
            }
        }

        //protected void AddAuthorizationCookie(NuVasiveUser user)
        //{
        //    var timeout = Int32.Parse(ConfigurationManager.AppSettings["AUTH_COOKIE_TIEMOUT"]);
        //    var ticket = new FormsAuthenticationTicket(user.UserName, false, timeout);
        //    var encrypted = FormsAuthentication.Encrypt(ticket);
        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
        //    {
        //        Expires = System.DateTime.Now.AddMinutes(timeout),
        //        Domain = ConfigurationManager.AppSettings["AuthCookieDomain"]
        //    };

        //    HttpContext.Response.Cookies.Add(cookie);
        //    HttpContext.User = user.ToPrincipal();
        //}
    }
}
