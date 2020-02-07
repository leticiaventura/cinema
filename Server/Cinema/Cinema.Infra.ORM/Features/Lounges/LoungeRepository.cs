using System.Linq;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.Infra.ORM.Base;

namespace Cinema.Infra.ORM.Features.Lounges
{
    public class LoungeRepository : AbstractRepository<Lounge>, ILoungeRepository
    {
        public LoungeRepository(BaseContext context) : base(context)
        {
        }

        public bool IsNameAlreadyInUse(string name, long loungeId)
        {
            return _context.Lounges.Where(x => x.Name.Equals(name) && x.Id != loungeId).Count() > 0;
        }
    }
}
