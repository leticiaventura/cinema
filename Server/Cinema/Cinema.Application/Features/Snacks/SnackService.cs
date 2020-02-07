using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;

namespace Cinema.Application.Features.Snacks
{
    public class SnackService : AbstractService<Snack>, ISnackService
    {
        public SnackService(ISnackRepository repository, Mapper mapper) : base(repository, mapper)
        {
        }

        public bool IsNameAlreadyInUse(string name, long id)
        {
            return ((ISnackRepository)_repository).IsNameAlreadyInUse(name, id);
        }
    }
}
