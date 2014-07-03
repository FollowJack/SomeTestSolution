using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace PingYourPackage.API.Config
{
    public class AutofacWebAPI
    {
        //for hosting initialisezing
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config,RegisterServices(new ContainerBuilder()));
        }
        //for integration tests
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Registration

            return builder.Build();
        }
    }
}
