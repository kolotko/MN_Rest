using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/proxytest", async () =>
{
    try
    {
        HttpResponseMessage response = await MakeRequestUsingRandomProxy("https://httpbin.org/ip");
        string responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);
    }
    catch (Exception e)
    {
        Console.WriteLine("Request failed with error: ", e.Message);
    }
});

app.Run();

static async Task<HttpResponseMessage> MakeRequestUsingRandomProxy(string url)
{
    List<string> proxies = new List<string>
        { 
            "http://103.195.65.209:8080",
        };
    
    // get random proxy
    Random random = new Random();
    int index = random.Next(proxies.Count);
    string proxyURL = proxies[index];
  
    // set proxy
    HttpClientHandler handler = new HttpClientHandler()
    {
        Proxy = new WebProxy(proxyURL)
    };

    // send request
    using (HttpClient client = new HttpClient(handler))
    {
        return await client.GetAsync(url);
    }
}