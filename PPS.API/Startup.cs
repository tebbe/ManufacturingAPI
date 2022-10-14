using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using PPS.API.HelperClasses;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

[assembly: OwinStartup(typeof(PPS.API.Startup))]

namespace PPS.API
{
    public partial class Startup
    { 
        public void Configuration(IAppBuilder app)
        {
            // Enable cors origin requests
            app.UseCors(CorsOptions.AllowAll);
            ConfigureAuth(app);
        }
    }
}
