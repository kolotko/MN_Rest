using System.Net;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace RetriesImplementation;

public static class PolicyHandlers
{
    public static IAsyncPolicy<HttpResponseMessage> GetStandardPolly()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(x => x.StatusCode is >= HttpStatusCode.InternalServerError or HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));
    }
}