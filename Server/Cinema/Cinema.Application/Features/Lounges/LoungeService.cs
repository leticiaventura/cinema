using System;
using System.Linq;
using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Domain.Common;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;

namespace Cinema.Application.Features.Lounges
{
    public class LoungeService : AbstractService<Lounge>, ILoungeService
    {
        ISessionRepository _sessionRepository;
        public LoungeService(ISessionRepository sessionRepository, ILoungeRepository repository, Mapper mapper) : base(repository, mapper)
        {
            _sessionRepository = sessionRepository;
        }



        public bool IsNameAlreadyInUse(string name, long loungeId)
        {
            return ((ILoungeRepository)_repository).IsNameAlreadyInUse(name, loungeId);
        }

        public override bool Remove(long id)
        {
            if (GetSessions(id).Where(x => x.End >= DateTime.Now).Any())
                throw new BusinessException(ErrorCodes.BadRequest, "Não é possível deletar uma sala que possua sessões futuras vinculadas à ela.");
            return base.Remove(id);
        }

        public override bool Update(AbstractUpdateCommand<Lounge> command)
        {

            if (GetSessions(command.Id).Where(x => x.End >= DateTime.Now).Any())
                throw new BusinessException(ErrorCodes.BadRequest, "Não é possível editar uma sala que possua sessões futuras vinculadas à ela.");
            return base.Update(command);
        }

        private IQueryable<Session> GetSessions(long id)
        {
            return _sessionRepository.GetAll().Where(x => x.LoungeId == id);
        }
    }
}
