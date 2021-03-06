﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PingYourPackage.API.MessageHandler
{
    public class RequireHttpsMessageHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                HttpResponseMessage forbiddenResponse = request.CreateResponse(HttpStatusCode.Forbidden);
                forbiddenResponse.ReasonPhrase = "SSL Required";
                //Return forbidden Http response
                return Task.FromResult<HttpResponseMessage>(forbiddenResponse);
            }
            //go on at the request pipeline
            return base.SendAsync(request, cancellationToken);
        }
    }
}
