using System.Linq;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.Infra.ORM.Base;

namespace Cinema.Infra.ORM.Features.Movies
{
    public class MovieRepository : AbstractRepository<Movie>, IMovieRepository
    {
        public MovieRepository(BaseContext context) : base(context)
        {
        }

        public bool IsNameAlreadyInUse(string name, long movieId)
        {
            return _context.Movies.Where(x => x.Name.Equals(name) && x.Id != movieId).Count() > 0;
        }
    }
}
