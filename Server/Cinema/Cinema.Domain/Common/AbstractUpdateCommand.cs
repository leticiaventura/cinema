
using FluentValidation.Results;

namespace Cinema.Domain.Common
{
    public abstract class AbstractUpdateCommand<T> where T : Entity
    {
        public virtual long Id { get; set; }

        public abstract ValidationResult Validate(IService<T> service);
    }
}
