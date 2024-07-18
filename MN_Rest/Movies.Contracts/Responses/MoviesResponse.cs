namespace Movies.Contracts.Responses;

public record MoviesResponse
{
    public required IEnumerable<MovieResponse> Items { get; init; } = Enumerable.Empty<MovieResponse>();
}