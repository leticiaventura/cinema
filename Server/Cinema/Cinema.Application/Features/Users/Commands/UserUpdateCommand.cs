using Cinema.Domain.Common;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Users.Commands
{
    public class UserUpdateCommand : AbstractUpdateCommand<User>
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }

        public override ValidationResult Validate(IService<User> service)
        {
            return new Validator(service as IUserService).Validate(this);
        }

        class Validator : AbstractValidator<UserUpdateCommand>
        {
            public Validator(IUserService service)
            {
                RuleFor(c => c.Id).GreaterThan(0);;
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Email).NotNull().NotEmpty().Must((c, email) => !service.IsEmailAlreadyInUse(email, c.Id)).WithMessage("O email já está em uso");
            }
        }
    }
}
