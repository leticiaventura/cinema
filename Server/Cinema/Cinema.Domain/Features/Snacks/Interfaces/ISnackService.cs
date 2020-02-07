using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Snacks.Interfaces
{
    public interface ISnackService : IService<Snack>
    {
        bool IsNameAlreadyInUse(string name, long id);
    }
}
