using PPS.Service.ServiceInterfaces;
using PPS.Service.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace PPS.API.HelperClasses
{
    public class TimingActionFilter : ActionFilterAttribute
    {
        private const string Key = "__action_duration__";
        PPS.Shared.Core.HelperClasses.Logger _logger;
        private readonly IUserService _userService;
        public TimingActionFilter()
        {
            _userService = new UserService();
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (SkipLogging(actionContext))
            {
                return;
            }

            var stopWatch = new Stopwatch();
            actionContext.Request.Properties[Key] = stopWatch;
            stopWatch.Start();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            _logger = new PPS.Shared.Core.HelperClasses.Logger();
            if (!actionExecutedContext.Request.Properties.ContainsKey(Key))
            {
                return;
            }
            string userName = null;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            var stopWatch = actionExecutedContext.Request.Properties[Key] as Stopwatch;
            var requestUri = actionExecutedContext.Request.RequestUri.AbsoluteUri;
            var referrerUri = actionExecutedContext.Request.Headers.Referrer.AbsoluteUri;
            if (stopWatch != null)
            {
                stopWatch.Stop();

                var actionName = actionExecutedContext.ActionContext?.ActionDescriptor?.ActionName ?? "";
                var controllernName = actionExecutedContext.ActionContext?.ActionDescriptor?.ControllerDescriptor?.ControllerName ?? "";
                var user = _userService.GetUserByEmailId(userName);
                if (user != null)
                {
                    //_logger.UserActivityLog(user.Id, userName, controllernName, actionName, Convert.ToDecimal(stopWatch.Elapsed.TotalSeconds), requestUri, referrerUri);
                }
                else
                {
                    //_logger.UserActivityLog(userName, controllernName, actionName, Convert.ToDecimal(stopWatch.Elapsed.TotalSeconds), requestUri, referrerUri);
                }
                _logger.UserActivityLog(userName, controllernName, actionName, Convert.ToDecimal(stopWatch.Elapsed.TotalSeconds), requestUri, referrerUri);
            }
        }

        private static bool SkipLogging(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoLogAttribute>().Any() ||
                    actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<NoLogAttribute>().Any();
        }
    }
}