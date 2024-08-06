var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/test", () =>
{
    CallCount.Count++;
    Console.WriteLine("call: " + CallCount.Count);
    if (CallCount.Count % 3 == 0)
    {
        return Results.Ok();
    }
    
    throw new InvalidOperationException("Sample Exception");
});

app.Run();

public static class CallCount
{
    public static int Count { get; set; }
}
