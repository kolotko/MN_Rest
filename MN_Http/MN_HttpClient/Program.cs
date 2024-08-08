using MN_HttpClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<IAddHttpClientTypeExample, AddHttpClientTypeExample>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

builder.Services.AddHttpClient("default", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

var app = builder.Build();

app.MapGet("/theworstway", async () =>
{
    using var httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
    };
    return await httpClient.GetFromJsonAsync<User>("todos/1");
});

app.MapGet("/worseway", async () =>
{
    return await StaticHttpClient.Client.GetFromJsonAsync<User>("todos/1");
});

app.MapGet("/betterway", async (IAddHttpClientTypeExample addHttpClientTypeExample) =>
{
    return await addHttpClientTypeExample.Call();
});

app.MapGet("/alsogoodway", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient("default");
    return await httpClient.GetFromJsonAsync<User>("todos/1");
});

app.Run();

public class User
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public bool Completeditle { get; set; }
}