using System.Linq;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;
using Cinema.Infra.ORM.Base;
using System.Data.Entity;

namespace Cinema.Infra.ORM.Features.Sessions
{
    public class SessionRepository : AbstractRepository<Session>, ISessionRepository
    {
        public SessionRepository(BaseContext context) : base(context)
        {
        }

        public override IQueryable<Session> GetAll(int size)
        {
            return _context.Sessions.Include(s => s.Movie).Include(s => s.Lounge).Take(size);
        }

        public override IQueryable<Session> GetAll()
        {
            return _context.Sessions.Include(s => s.Movie).Include(s => s.Lounge);
        }

        public override Session GetById(long id)
        {
            return _context.Sessions.Include(s=> s.Movie).Include(s => s.Lounge).FirstOrDefault(e => e.Id == id);
        }
    }
}
