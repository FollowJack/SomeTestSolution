using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PingYourPackage.API.MessageHandler;
using PingYourPackage.API.Test.TestHelper;

namespace PingYourPackage.API.Test.MessageHandler
{
    [TestFixture]
    public class RequireHttpsMessageHandlerTests
    {
        [Test]
        public async Task Returns_Forbidden_If_Request_Is_Not_Over_HTTPS()
        {
            // Arange - create new HTTP GET request 
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080");
            // Arange - create HTTPS MessageHandler
            var requireHtttpsMessageHandler = new RequireHttpsMessageHandler();

            // Act - Invoke async request
            var response = await requireHtttpsMessageHandler.InvokeAsync(request);

            // Assert - Statuscode is Forbidden
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Test]
        public async Task Returns_Delegated_StatusCode_When_Request_Is_Over_HTTPS()
        {
            // Arange - create new HTTPS GET request
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:8080");
            // Arange - create HTTPS MessageHandler
            var requireHtttpsMessageHandler = new RequireHttpsMessageHandler();

            // Act - Invoke async request
            var response = await requireHtttpsMessageHandler.InvokeAsync(request);

            // Assert - Statuscode is OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
