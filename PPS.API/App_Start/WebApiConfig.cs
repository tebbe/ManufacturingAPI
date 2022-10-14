using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using PPS.API.HelperClasses;
using System.Web.Http.Cors;
using System.Web.Configuration;
using PPS.API.Attributes;

namespace PPS.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var enabledCors = bool.Parse(WebConfigurationManager.AppSettings["EnableCORS"]);
            if (enabledCors)
            {
                var cors = new EnableCorsAttribute("*", "*", "*");
                //var cors = new EnableCorsAttribute("http://pps.ramate.net", "*", "*");
                config.EnableCors(cors);
            }
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Add Authorize attribute
            config.Filters.Add(new PPSAuthorizeAttribute());
        
            // Add timer attribute 
            config.Filters.Add(new TimingActionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
