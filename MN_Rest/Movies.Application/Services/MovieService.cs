using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(
    IMovieRepository movieRepository, 
    IValidator<Movie> movieValidator, 
    IRatingRepository ratingRepository,
    IValidator<GetAllMoviesOptions> optionsValidator) : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        return await movieRepository.CreateAsync(movie, token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, Guid? userid = default, CancellationToken token = default)
    {
        return movieRepository.GetByIdAsync(id, userid, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, Guid? userid = default, CancellationToken token = default)
    {
        return movieRepository.GetBySlugAsync(slug, userid, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
    {
        await optionsValidator.ValidateAndThrowAsync(options, token);
        return await movieRepository.GetAllAsync(options, token);
    }

    public Task<int> GetCountAsync(string? title, int? year, CancellationToken token = default)
    {
        return movieRepository.GetCountAsync(title, year, token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, Guid? userid = default, CancellationToken token = default)
    {
        await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await movieRepository.ExistsByIdAsync(movie.Id, token);
        if (!movieExists)
        {
            return null;
        }

        await movieRepository.UpdateAsync(movie, token);

        if (!userid.HasValue)
        {
            var rating = await ratingRepository.GetRatingAsync(movie.Id, token);
            movie.Rating = rating;
            return movie;
        }
        
        var ratings = await ratingRepository.GetRatingAsync(movie.Id, userid.Value, token);
        movie.Rating = ratings.Rating;
        movie.UserRating = ratings.UserRating;
        return movie;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return movieRepository.DeleteByIdAsync(id, token);
    }
}