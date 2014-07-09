using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Entities.Core;
using PingYourPackage.Domain.Services;

namespace PingYourPackage.API.Config
{
    public class AutofacWebAPI
    {
        //for hosting initialisezing
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }
        //for integration tests
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            //EF DbContext
            builder.RegisterType<PingYourPackageEntities>().As<DbContext>().InstancePerRequest();

            //Repositories
            builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IEntityRepository<>)).InstancePerRequest();

            //Services
            builder.RegisterType<MembershipService>().As<IMembershipService>().InstancePerRequest();
            builder.RegisterType<ShipmentService>().As<IMembershipService>().InstancePerRequest();

            return builder.Build();
        }
    }
}
