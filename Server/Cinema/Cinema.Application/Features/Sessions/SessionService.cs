using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Domain.Common;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;

namespace Cinema.Application.Features.Sessions
{
    public class SessionService : AbstractService<Session>, ISessionService
    {
        IMovieService _movieService;
        ILoungeService _loungeService;

        public SessionService(IMovieService movieService, ILoungeService loungeService, ISessionRepository repository, Mapper mapper) : base(repository, mapper)
        {
            _movieService = movieService;
            _loungeService = loungeService;
        }

        public override Session Add(AbstractAddCommand<Session> command)
        {
            Session session = PopulateSession(command);
            session.Validate();

            if (!GetAvailableLounges(session).Select(x => x.Id).Contains(session.LoungeId))
                throw new BusinessException(ErrorCodes.BadRequest, "A sala selecionada não está disponível.");

            var newSession = _repository.Add(session);
            return new Session { Id = newSession.Id };
        }


        public IQueryable<Lounge> GetAvailableLounges(Session session)
        {
            IQueryable<Session> futureSessions = GetAll().Where(x => x.Start > DateTime.Now);

            var occupiedLounges = futureSessions.Where(CrossSchedule(session)).Select(futureSession => futureSession.Lounge.Id);
            var availableLounges = _loungeService.GetAll().Where(x => !occupiedLounges.Contains(x.Id));

            return availableLounges;
        }

        public override bool Remove(long id)
        {
            var session = GetById(id);
            if (session != null && session.Start.AddDays(-10) <= DateTime.Now)
            {
                throw new BusinessException(ErrorCodes.BadRequest, "A sessão só pode ser excluída com mais de 10 dias de antecedência");
            }
            return base.Remove(id);
        }

        private static Expression<Func<Session, bool>> CrossSchedule(Session session)
        {
            return futureSession => (session.Start >= futureSession.Start && session.Start <= futureSession.End) || (session.End >= futureSession.Start && session.End <= futureSession.End);
        }

        private Session PopulateSession(AbstractAddCommand<Session> command)
        {
            Session session = _mapper.Map<Session>(command);
            session.Movie = _movieService.GetById(session.MovieId);
            session.Lounge = _loungeService.GetById(session.LoungeId);
            session.End = session.Start.AddMinutes(session.Movie.Length);
            return session;
        }
    }
}
