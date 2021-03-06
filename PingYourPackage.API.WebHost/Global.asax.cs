﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using PingYourPackage.API.Config;
using PingYourPackage.Domain.Entities.Configuration;

namespace PingYourPackage.API.WebHost
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;
            
            RouteConfig.RegisterRoutes(config);
            WebAPIConfig.Configure(config);
            DbInitializer.Initialize();
            AutofacWebAPI.Initialize(config);
        }
    }
}
