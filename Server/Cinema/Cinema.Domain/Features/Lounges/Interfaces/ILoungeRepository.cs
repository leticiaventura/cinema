using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Lounges.Interfaces
{
    public interface ILoungeRepository : IRepository<Lounge>
    {
        bool IsNameAlreadyInUse(string name, long loungeId);

    }
}
