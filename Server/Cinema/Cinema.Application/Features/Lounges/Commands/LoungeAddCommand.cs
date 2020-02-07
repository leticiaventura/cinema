using Cinema.Domain.Common;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Lounges.Commands
{
    public class LoungeAddCommand : AbstractAddCommand<Lounge>
    {
        public virtual string Name { get; set; }
        public virtual int Seats { get; set; }

        public override ValidationResult Validate(IService<Lounge> service)
        {
            return new Validator(service as ILoungeService).Validate(this);
        }

        class Validator : AbstractValidator<LoungeAddCommand>
        {
            public Validator(ILoungeService service)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().Must((name) => !service.IsNameAlreadyInUse(name, 0)).WithMessage("O nome já está em uso");
                RuleFor(c => c.Seats).GreaterThanOrEqualTo(20).LessThanOrEqualTo(100);
            }
        }
    }
}
