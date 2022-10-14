using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using PPS.API.HelperClasses;
using PPS.API.Models;
using PPS.API.Shared.ViewModel.User;
using PPS.Core;
using PPS.Data.Dtos.User;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Shared.Core.HelperClasses;
using PPS.Shared.Service.ServiceInterfaces;
using PPS.Shared.Service.Services;
using PPS.Shared.Service.Vm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PPS.API.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : BaseApiController
    {
        private IUserService _userService;
        private ApplicationUserManager _userManager;
        private readonly IEmailService _emailService;
        PPS.Shared.Core.HelperClasses.ILogger _logger;

        public UserController()
        {
            _userService = new UserService();
            _logger = new Logger();
            _emailService = new EmailService();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Route("GetUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            try
            {
                var currentUser = await UserId();
                var users = _userService.GetUsers(currentUser);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("GetUserRoleById/{userId}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserRoleById(int userId)
        {
            try
            {
                var user = await _userService.GetUserRoleById(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserRegisterVm user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newUser = new ApplicationUser() { UserName = user.Email.Trim(), Email = user.Email.Trim() };
                //var password = "Aa123#";
                var userTempPassword = GenerateTemporaryPassword();

                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

                IdentityResult result = await UserManager.CreateAsync(newUser, userTempPassword);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                //var userTempPasswordKey = GenerateTemporaryPasswordKey();                
                //var encryptedPassword = AESThenHMAC.EncryptWithPassword(userTempPassword, userTempPasswordKey);
                //user.Password = encryptedPassword;
                //user.PasswordKey = userTempPasswordKey;
                //var aspNetUser = await userManager.FindByIdAsync(user)
                var hasCreatedUser = _userService.Register(user);
                if(hasCreatedUser == true)
                {
                    var bodyStart = "Your user registration is successfully complated at PPS Plastic Industris Ltd.";
                    var subject = "Registration is successfully completed.";
                    await SendEmail(user.Email.Trim(), userTempPassword, bodyStart, subject);
                }
                return Ok(hasCreatedUser);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }        

        [Route("ResetUser/{aspNetUserId}")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetUser(string aspNetUserId)
        {
            try
            {
                var userTempPassword = GenerateTemporaryPassword();
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await userManager.FindByIdAsync(aspNetUserId);

                string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var result = await UserManager.ResetPasswordAsync(user.Id, resetToken, userTempPassword);

                if (!result.Succeeded)
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, GetErrorResult(result).ToString()));
                }
                var bodyStart = "Your password has been reset successfully!";
                var subject = "Password reset is successfully completed.";
                await SendEmail(user.Email, userTempPassword, bodyStart, subject);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UserLock/{userId}")]
        [HttpPost]
        public IHttpActionResult UserLock(int userId)
        {
            try
            {
                var locked = _userService.UserLock(userId);
                return Ok(locked);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UserUnlock/{userId}")]
        [HttpPost]
        public IHttpActionResult UserUnlock(int userId)
        {
            try
            {
                var unlocked = _userService.UserUnock(userId);
                return Ok(unlocked);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UserActivate/{userId}")]
        [HttpPost]
        public IHttpActionResult UserActivate(int userId)
        {
            try
            {
                var active = _userService.UserActivate(userId);
                return Ok(active);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UserDeactivate/{userId}")]
        [HttpPost]
        public IHttpActionResult UserDeactivate(int userId)
        {
            try
            {
                var deactive = _userService.UserDeactivate(userId);
                return Ok(deactive);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("UpdateUser")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(UserVm userVm)
        {
            try
            {
                var vm = await UserId();
                userVm.AssignedUserId = vm.Id;
                await _userService.UpdateUser(userVm);
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.Log(RequestContext.Principal.Identity.Name, Request.RequestUri.AbsolutePath, ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private string GenerateTemporaryPassword()
        {
            var password = "";
            var random = new Random();
            var upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var lowerCase = "abcdefghijklmnopqrstuvwxyz";
            var specialCase = "!@#$%&";
            password = upperCase[random.Next(upperCase.Length)].ToString();
            password += lowerCase[random.Next(lowerCase.Length)].ToString();
            password += random.Next(10000, 100000).ToString();
            password += specialCase[random.Next(specialCase.Length)].ToString();
            return password;
        }

        private string GenerateTemporaryPasswordKey()
        {
            var passwordKey = Guid.NewGuid().ToString();

            return passwordKey;
        }

        private async Task SendEmail(string email, string userTempPassword, string bodyStart, string subject)
        {
            var htmlBody = new StringBuilder("");
            htmlBody.Append(bodyStart);
            htmlBody.Append("<br/>");
            htmlBody.Append("Use the following password to login into system.");
            htmlBody.Append("<br/><br/><br/>");
            htmlBody.Append("User email: " + email);
            htmlBody.Append("<br/>");
            htmlBody.Append("Password: " + userTempPassword);
            var vm = new EmailVm
            {
                ToAddress = new MailAddress(email),
                Subject = subject,
                Body = htmlBody.ToString()
            };
            await _emailService.SendRegisterEmail(vm);
        }
        #endregion
    }
}
