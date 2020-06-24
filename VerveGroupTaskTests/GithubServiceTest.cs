using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerveGroupTask.Web.Services;

namespace VerveGroupTask.Tests
{
    public class GithubServiceTest
    {
        private Mock<HttpMessageHandler> CreateHttpMoc()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpResponseMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                })
                .Verifiable();
            return mock;
        }

        //private GithubService CreateService(Mock<HttpMessageHandler> mock)
        //{
           // var client = new HttpClient(mock.Object);
            //var service = new GithubService(client);
            //return service;
        //}
    }
}
