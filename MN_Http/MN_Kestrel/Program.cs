var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();