using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PPS.API.Models;
using PPS.API.Controllers;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using PPS.API.Shared.ViewModel.User;
using System.Collections.Concurrent;
using PPS.Data.Edmx;
using PPS.API.Shared.Utility;
using PPS.Shared.Service.Vm;
using PPS.Shared.Service.Services;

namespace PPS.API.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly IPolicyService _policyService;
        private readonly IUserService _userService;
        private readonly static string companyId = ConfigUtils.GetSafeAppSettingValue("CompanyId");

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
            _policyService = new PolicyService();
            _userService = new UserService();           
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var logger = new PPS.Shared.Core.HelperClasses.Logger();            
            try
            {
                var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
                var dbUser = await _userService.GetUserByAspNetUserId(user.Id);
                if (user == null || dbUser.CompanyId.ToString() != companyId)
                {
                    logger.UserLoginLog(context.UserName, false, "The user name or password is incorrect.");
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
                logger.UserLoginLog(context.UserName, true, "");

                var companyName = dbUser.Company.FullName;
                var companyAddress = dbUser.Company.Address;
                var companyEmail = dbUser.Company.Email;
                var companyContact = dbUser.Company.Phone;
                var companyLogoPath = dbUser.Company.LogoPath;

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                var policies = await _policyService.GetPolicyByUser(user.Id);
                var policyCode = policies.Select(x => x.PolicyCode).ToList();
                AuthenticationProperties properties = CreateProperties(policyCode, user.Email, _publicClientId, companyName, companyAddress,companyEmail,companyContact,companyLogoPath);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch(Exception ex)
            {
                logger.UserLoginLog(context.UserName, false, ex.Message);
                logger.Log(context.UserName, "login", ex);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(List<int> policyCode, string email, string publicClientId, string companyName,string companyAddress,string companyEmail,string companyContact,string companyLogoPath)
        {            
            var policies = new Dictionary<string, string>
            {
                { "userName", email },
                { "policies", string.Join(",", policyCode) },
                { "companyId", companyId},
                { "fiscalYear", "2018"},
                { "client_id", publicClientId},
                { "companyName", companyName },
                { "companyAddress", companyAddress },
                { "companyEmail", companyEmail},
                { "companyContact", companyContact },
                { "companyLogoPath", companyLogoPath }
            };

            return new AuthenticationProperties(policies);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.OwinContext.Get<string>("as:client_id");

            // enforce client binding of refresh token
            if (originalClient != currentClient)
            {
                context.Rejected();
                return;
            }

            // chance to change authentication ticket for refresh token requests
            var newId = new ClaimsIdentity(context.Ticket.Identity);
            newId.AddClaim(new Claim("newClaim", "refreshToken"));

            var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
            context.Validated(newTicket);
        }
    }
}