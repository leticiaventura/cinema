using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Movies.Interfaces
{
    public interface IMovieService : IService<Movie>
    {
        bool IsNameAlreadyInUse(string name, long movieId);
    }
}
