using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Movies.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        bool IsNameAlreadyInUse(string name, long movieId);
    }
}
