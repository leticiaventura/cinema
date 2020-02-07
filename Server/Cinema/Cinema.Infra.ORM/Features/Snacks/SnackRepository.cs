using System.Linq;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;
using Cinema.Infra.ORM.Base;

namespace Cinema.Infra.ORM.Features.Snacks
{
    public class SnackRepository : AbstractRepository<Snack>, ISnackRepository
    {
        public SnackRepository(BaseContext context) : base(context)
        {
        }

        public bool IsNameAlreadyInUse(string name, long id)
        {
            return _context.Movies.Where(x => x.Name.Equals(name) && x.Id != id).Count() > 0;
        }
    }
}
