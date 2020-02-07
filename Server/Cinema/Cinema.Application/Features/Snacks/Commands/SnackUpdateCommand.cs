using Cinema.Domain.Common;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Snacks.Commands
{
    public class SnackUpdateCommand : AbstractUpdateCommand<Snack>
    {
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual string Image { get; set; }

        public override ValidationResult Validate(IService<Snack> service)
        {
            return new Validator(service as ISnackService).Validate(this);
        }

        class Validator : AbstractValidator<SnackUpdateCommand>
        {
            public Validator(ISnackService service)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().Must((c, name) => !service.IsNameAlreadyInUse(name, c.Id)).WithMessage("O nome já está em uso");
                RuleFor(c => c.Image).NotNull().NotEmpty().WithMessage("É necessário inserir uma imagem.");
                RuleFor(c => c.Id).GreaterThan(0);
            }
        }
    }
}
