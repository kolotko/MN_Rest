using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator) : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        return await movieRepository.CreateAsync(movie, token);
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await movieRepository.GetByIdAsync(id, token);
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return await movieRepository.GetBySlugAsync(slug, token);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
    {
        return await movieRepository.GetAllAsync(token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        await movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
        var movieExists = await movieRepository.ExistsByIdAsync(movie.Id, token);
        if (!movieExists)
        {
            return null;
        }
        
        await movieRepository.UpdateAsync(movie, token);
        return movie;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return await movieRepository.DeleteByIdAsync(id, token);
    }
}