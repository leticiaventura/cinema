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
        public virtual int Rows { get; set; }
        public virtual int Columns { get; set; }

        public override ValidationResult Validate(IService<Lounge> service)
        {
            return new Validator(service as ILoungeService).Validate(this);
        }

        class Validator : AbstractValidator<LoungeAddCommand>
        {
            public Validator(ILoungeService service)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().Must((name) => !service.IsNameAlreadyInUse(name, 0)).WithMessage("O nome já está em uso");
                RuleFor(c => c.Rows).GreaterThanOrEqualTo(5).LessThanOrEqualTo(10);
                RuleFor(c => c.Columns).GreaterThanOrEqualTo(4).LessThanOrEqualTo(10);
            }
        }
    }
}
