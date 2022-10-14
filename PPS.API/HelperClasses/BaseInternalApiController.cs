using Microsoft.AspNet.Identity;
using PPS.API.Attributes;
using PPS.API.Shared.ViewModel.User;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace PPS.API.HelperClasses
{
    [PPSInternalApiAuthorize]
    public class BaseInternalApiController : ApiController
    {
        private readonly IUserService _userService;
        public BaseInternalApiController()
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
    }
}
