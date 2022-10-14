using Microsoft.AspNet.Identity;
using PPS.API.Shared.Utility;
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
    public class PPSInternalApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
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
            //bool isAuthorized = base.IsAuthorized(actionContext);

            //if (isAuthorized)
            //{
            //var aspNetUserId = UserId;

            //if (string.IsNullOrEmpty(aspNetUserId) || actionContext.ActionDescriptor == null || actionContext.ActionDescriptor.ControllerDescriptor == null)
            //{
            //    return false;
            //}
            var actionAccessKey = actionContext.ControllerContext.RouteData.Values["AccessKey"];
            var actionKey = ConfigUtils.GetSafeAppSettingValue("AccessKey");
            return actionAccessKey.Equals(actionKey);
            //}
            //return false;
        }
    }
}