using Polly;
using Polly.Contrib.WaitAndRetry;
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

app.Run();