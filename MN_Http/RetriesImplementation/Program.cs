using System.Net;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Registry;
using Polly.Retry;
using RetriesImplementation;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("StandardPoli",client =>
{
    client.BaseAddress = new Uri("http://localhost:64593");
}).AddPolicyHandler(PolicyHandlers.GetStandardPolly());

builder.Services.AddHttpClient("BestpracticePoli", client =>
{
    client.BaseAddress = new Uri("http://localhost:64593");
}).AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5)));

builder.Services.AddResiliencePipeline("default", x =>
{
    x.AddRetry(new RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        Delay = TimeSpan.FromSeconds(2),
        MaxRetryAttempts = 2,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true
    })
    .AddTimeout(TimeSpan.FromSeconds(30));
});

builder.Services.AddHttpClient("standardresiliencehandler", client =>
{
    client.BaseAddress = new Uri("http://localhost:64593");
}).AddStandardResilienceHandler();

var app = builder.Build();

app.MapGet("/standardpoli", async (IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("StandardPoli");
    var response = await client.GetAsync("/test");
    var x = response;
});

app.MapGet("/bestpracticepoli", async (IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("BestpracticePoli");
    var response = await client.GetAsync("/test");
    var x = response;
});

app.MapGet("/resiliencepipelineprovider", async (ResiliencePipelineProvider<string> pipelineProvider, IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient();
    var pipeline = pipelineProvider.GetPipeline("default");
    var response = await pipeline.ExecuteAsync( async ct =>
    {
        var response = await client.GetAsync("http://localhost:64593/test", ct);
        if (response.StatusCode >= HttpStatusCode.InternalServerError)
        {
            throw new Exception();
        }

        return response;
    });
    var x = response;
});

app.MapGet("/standardresiliencehandler", async (IHttpClientFactory httpFactory) =>
{
    var client = httpFactory.CreateClient("standardresiliencehandler");
    var response = await client.GetAsync("/test");
    var x = response;
});

 
app.Run();