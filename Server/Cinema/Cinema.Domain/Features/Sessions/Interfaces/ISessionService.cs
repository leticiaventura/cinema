using System.Linq;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Lounges;

namespace Cinema.Domain.Features.Sessions.Interfaces
{
    public interface ISessionService : IService<Session>
    {
        IQueryable<Lounge> GetAvailableLounges(Session session);
    }
}
