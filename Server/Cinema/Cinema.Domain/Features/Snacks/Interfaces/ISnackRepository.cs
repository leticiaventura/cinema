using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Snacks.Interfaces
{
    public interface ISnackRepository : IRepository<Snack>
    {
        bool IsNameAlreadyInUse(string name, long id);
    }
}
