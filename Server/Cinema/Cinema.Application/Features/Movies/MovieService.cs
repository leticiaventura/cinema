using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;

namespace Cinema.Application.Features.Movies
{
    public class MovieService : AbstractService<Movie>, IMovieService
    {
        public MovieService(IMovieRepository repository, Mapper mapper) : base(repository, mapper)
        {
        }

        public bool IsNameAlreadyInUse(string name, long movieId)
        {
            return ((IMovieRepository)_repository).IsNameAlreadyInUse(name, movieId);
        }
    }
}
