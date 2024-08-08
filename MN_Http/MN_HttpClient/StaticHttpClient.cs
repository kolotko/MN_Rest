namespace MN_HttpClient;

public static class StaticHttpClient
{
    public static HttpClient Client { get; set; } = new HttpClient(new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(1)
    })
    {
        BaseAddress = new Uri("https://jsonplaceholder.typicode.com/")
    };
}