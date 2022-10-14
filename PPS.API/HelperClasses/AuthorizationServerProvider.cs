using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.Data;
using PPS.Core;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using PPS.Data.Edmx;
using PPS.Shared.Core.HelperClasses;

namespace PPS.API.HelperClasses
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IUserService _userSvc;
        private User _user;
        private int _fiscalYear; 
        ILogger _logger;
        public AuthorizationServerProvider()
        {
            _userSvc = new UserService();
            _user = new User();
            _fiscalYear = DateTime.Now.Year;
            _logger = new Logger();
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            //return base.ValidateClientAuthentication(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                _user = await _userSvc.GetUserByEmailId(context.UserName);
                if (_user == null)
                {
                    context.SetError("invalid_grant", "Invalid try.");
                    return;
                }
                if (_user.Locked == true)
                {
                    context.SetError("invalid_grant", "Your ID is locked.");
                    return;
                }
                var decryptedPassword = AESThenHMAC.DecryptWithPassword(_user.Password, _user.PasswordKey);

                if (context.Password != decryptedPassword)
                {
                    _userSvc.SetUserLoginInvalidCount(context.UserName);
                    context.SetError("invalid_grant", "Invalid try.");
                    return;
                } 
                else
                {
                    _userSvc.ResetUserLoginInvalidCount(context.UserName);
                }
                var roles = new List<Claim>();
                if (_user.UserRole != null)
                {
                    foreach (var r in _user.UserRole)
                    {
                        roles.Add(new Claim(ClaimTypes.Role, r.Role.RoleName));
                    }
                }
                identity.AddClaims(roles);
                //AuthenticationProperties properties = CreateProperties(_user.Email, Newtonsoft.Json.JsonConvert.SerializeObject(roles.Select(x => x.Value)));

                identity.AddClaim(new Claim("username", _user.Email));
                //HttpContext.Current.User.Identity.Name
                //identity.AddClaim(new Claim(ClaimTypes.Name, _user.FirstName + " " + _user.LastName));
                //var ticket = new AuthenticationTicket(identity, properties);
                context.Validated(identity);
            }
            catch(Exception ex)
            {
                _logger.Log(context.UserName, "Token", ex);
                context.SetError("invalid_grant", "Internal Error.");
                return;
            }           
        }
        //public static AuthenticationProperties CreateProperties(string userName, string Roles)
        //{
        //    IDictionary<string, string> data = new Dictionary<string, string>
        //    {
        //        { "userName", userName },
        //        {"roles",Roles}
        //    };
        //    return new AuthenticationProperties(data);
        //}

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            if (_user == null)
                return Task.FromResult<object>(null);

            string roles = string.Empty;
            foreach (var r in _user.UserRole)
            {
                roles += r.Role.RoleName + ",";
            }
            context.AdditionalResponseParameters.Add("roles", roles);

            context.AdditionalResponseParameters.Add("userId", _user.Id);

            context.AdditionalResponseParameters.Add("companyId", _user.CompanyId);

            context.AdditionalResponseParameters.Add("companyName", _user.Company.Name);

            context.AdditionalResponseParameters.Add("fiscalYear", _fiscalYear);
            
            var fullName = _user.FirstName + " " + _user.LastName;
            context.AdditionalResponseParameters.Add("fullName", fullName);

            // TODO: Need to maintain database allowing for total back day 
            var totalBackDaysForInput = 2;
            context.AdditionalResponseParameters.Add("totalBackDaysForInput", totalBackDaysForInput);

            var expiresIn = DateTime.Now;
            expiresIn.AddHours(context.Options.AccessTokenExpireTimeSpan.Hours);
            expiresIn.AddMinutes(context.Options.AccessTokenExpireTimeSpan.Minutes);
            expiresIn.AddSeconds(context.Options.AccessTokenExpireTimeSpan.Seconds);
            context.AdditionalResponseParameters.Add("expires_in", expiresIn);

            return Task.FromResult<object>(null);
        }
    }    
}