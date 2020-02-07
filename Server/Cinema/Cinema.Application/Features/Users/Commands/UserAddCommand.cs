using Cinema.Domain.Common;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Users.Commands
{
    public class UserAddCommand : AbstractAddCommand<User>
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual int PermissionLevel { get; set; }

        public override ValidationResult Validate(IService<User> service)
        {
            return new Validator(service as IUserService).Validate(this);
        }

        class Validator : AbstractValidator<UserAddCommand>
        {
            public Validator(IUserService service)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty();
                RuleFor(c => c.Email).NotNull().NotEmpty().Must((email) => !service.IsEmailAlreadyInUse(email, 0)).WithMessage("O email já está em uso");
                RuleFor(c => c.Password).NotNull().NotEmpty();
                RuleFor(c => c.PermissionLevel).GreaterThan(0);
            }
        }
    }
}
