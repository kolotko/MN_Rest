using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Movies.Contracts.Requests;
using Refit;

var services = new ServiceCollection();
// własny client factory, dodać pakiet
services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s =>
    {
        async Task<string> AuthorizationHeaderValueGetter(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken) => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync();

        return new RefitSettings
        {
            AuthorizationHeaderValueGetter = AuthorizationHeaderValueGetter
        };
    })
    .ConfigureHttpClient(x =>
        x.BaseAddress = new Uri("https://localhost:5001"));
var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movie = await moviesApi.GetMovieAsync("test1-2020");
var newMovie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
{
    Title = "Spiderman 2",
    YearOfRelease = 2002,
    Genres = new []{ "Action"}
});

await moviesApi.UpdateMovieAsync(newMovie.Id, new UpdateMovieRequest()
{
    Title = "Spiderman 2",
    YearOfRelease = 2002,
    Genres = new []{ "Action", "Adventure"}
});

await moviesApi.DeleteMovieAsync(newMovie.Id);

var request = new GetAllMoviesRequest
{
    Title = null,
    Year = null,
    SortBy = null,
    Page = 1,
    PageSize = 3
};

var movies = await moviesApi.GetMoviesAsync(request);

foreach (var movieResponse in movies.Items)
{
    Console.WriteLine(JsonSerializer.Serialize(movieResponse));
}
