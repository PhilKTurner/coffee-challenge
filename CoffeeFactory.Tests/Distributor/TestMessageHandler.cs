using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeChallenge.CoffeeFactory.Tests;

public class TestMessageHandler : HttpMessageHandler
{
    public HttpStatusCode StatusCodeToReturn { get; set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseMessage = new HttpResponseMessage(StatusCodeToReturn);
        return Task.FromResult(responseMessage);
    }
}