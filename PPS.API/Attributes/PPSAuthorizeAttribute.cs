using Microsoft.AspNet.Identity;
using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace PPS.API.Attributes
{
    public class PPSAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }

        internal string UserId
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                {
                    return HttpContext.Current.User.Identity.GetUserId();
                }
                return string.Empty;
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool isAuthorized = base.IsAuthorized(actionContext);

            if (isAuthorized)
            {
                var aspNetUserId = UserId;

                if (string.IsNullOrEmpty(aspNetUserId) || actionContext.ActionDescriptor == null || actionContext.ActionDescriptor.ControllerDescriptor == null)
                {
                    return false;
                }

                //Get info of resource being accessed 
                var controller = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var action = actionContext.ActionDescriptor.ActionName;
                var httpMethod = actionContext.Request.Method.Method;
                if (actionContext.Request.Method == HttpMethod.Delete || actionContext.Request.Method == HttpMethod.Put)
                {
                    return false;
                }
                //var policyService = DependencyResolver.Current.GetService<IPolicyService>();
                var policyService = new PolicyService();
                //Check users access 				
                return policyService.ValidateUsersAccess(aspNetUserId, controller, action, httpMethod);
                //return true;
            }

            return false;
        }
    }
}