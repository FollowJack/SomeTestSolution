using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using PingYourPackage.Domain.Services;
using WebApiDoodle.Web;

namespace PingYourPackage.API.HttpExtension
{

    internal static class HttpRequestMessageExtensions
    {
        internal static IMembershipService GetMembershipService(this HttpRequestMessage request)
        {
            return request.GetService<IMembershipService>();
        }

        private static TService GetService<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();

            TService service = (TService)dependencyScope.GetService(typeof(TService));

            return service;
        }
    }
}
