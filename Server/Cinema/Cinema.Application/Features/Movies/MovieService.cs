using System.Linq;
using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;

namespace Cinema.Application.Features.Movies
{
    public class MovieService : AbstractService<Movie>, IMovieService
    {
        ISessionRepository _sessionRepository;

        public MovieService(ISessionRepository sessionRepository, IMovieRepository repository, Mapper mapper) : base(repository, mapper)
        {
            _sessionRepository = sessionRepository;
        }

        public bool IsNameAlreadyInUse(string name, long movieId)
        {
            return ((IMovieRepository)_repository).IsNameAlreadyInUse(name, movieId);
        }

        public override bool Remove(long id)
        {
            if (GetSessions(id).Any())
                throw new BusinessException(ErrorCodes.BadRequest, "Não é possível deletar um filme que já esteja vinculado a sessões.");
            return base.Remove(id);
        }

        private IQueryable<Session> GetSessions(long id)
        {
            return _sessionRepository.GetAll().Where(x => x.MovieId == id);
        }
    }
}
