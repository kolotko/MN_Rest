using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public partial class Movie
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string Slug => GenereteSlug();
    public required int YearOfRelease { get; set; }
    public required List<string> Genres { get; init; } = new();
    private string GenereteSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}