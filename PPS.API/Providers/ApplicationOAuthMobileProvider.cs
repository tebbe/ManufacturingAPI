using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PPS.API.Models;
using PPS.API.Shared.Utility;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PPS.API.Providers
{
    public class ApplicationOAuthMobileProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly IPolicyService _policyService;
        private readonly IUserService _userService;
        private readonly static string companyId = ConfigUtils.GetSafeAppSettingValue("CompanyId");

        public ApplicationOAuthMobileProvider(string publicClientId)
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

                var companyName = dbUser.Company.Name;
                var companyAddress = dbUser.Company.Address;
                var companyEmail = dbUser.Company.Email;
                var companyContact = dbUser.Company.Phone;
                var companyLogoPath = dbUser.Company.LogoPath;

                // Tested SMS service 
                //var smsVm = new SMSVm
                //{
                //    //User = ConfigUtils.GetSafeAppSettingValue("SmsPortalUser"),
                //    //Password = ConfigUtils.GetSafeAppSettingValue("SmsPortalPassword"),
                //    //Brand = ConfigUtils.GetSafeAppSettingValue("SmsPortalBrand"),
                //    SmsText = "Dear Customer/Dealer \nAssalamualaikum,\n\nYou have deposited Tk 15,000 to PPS through Pubali Bank on 30/12/2017.\nPlease confirm/check.\n\nIf you have any query, please let us know.\n\nThanks,\nPPS Plastic Pipe Ind.Ltd.\nCell: 01700703321-22, 25\nHotline: 01700703333",
                //    Numbers = new List<string> { "8801700703388", "16414510469" }
                //};
                //var sms = new SMSService();
                //await sms.SendSMS(smsVm);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                    CookieAuthenticationDefaults.AuthenticationType);
                var policies = await _policyService.GetPolicyByUser(user.Id);
                var policyCode = policies.Select(x => x.PolicyCode).ToList();
                AuthenticationProperties properties = CreateProperties(policyCode, user.Email, _publicClientId, companyName, companyAddress, companyEmail, companyContact, companyLogoPath);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }
            catch (Exception ex)
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

        public static AuthenticationProperties CreateProperties(List<int> policyCode, string email, string publicClientId, string companyName, string companyAddress, string companyEmail, string companyContact, string companyLogoPath)
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