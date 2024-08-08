namespace MN_HttpClient;

public interface IAddHttpClientTypeExample
{
    public Task<User> Call();
}
public class AddHttpClientTypeExample(HttpClient client) : IAddHttpClientTypeExample
{
    public async Task<User> Call()
    {
        return await client.GetFromJsonAsync<User>("todos/1");
    }
}