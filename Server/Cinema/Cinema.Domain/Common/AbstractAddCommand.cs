using FluentValidation.Results;

namespace Cinema.Domain.Common
{
    public abstract class AbstractAddCommand<T> where T : Entity
    {
        public abstract ValidationResult Validate(IService<T> service);
    }
}
