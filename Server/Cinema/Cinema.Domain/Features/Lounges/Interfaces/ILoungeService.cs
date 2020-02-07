using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Lounges.Interfaces
{
    public interface ILoungeService : IService<Lounge>
    {
        bool IsNameAlreadyInUse(string name, long loungeId);
    }
}
