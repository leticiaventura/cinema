using Cinema.Domain.Common;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Snacks.Commands
{
    public class SnackAddCommand : AbstractAddCommand<Snack>
    {
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual string Image { get; set; }

        public override ValidationResult Validate(IService<Snack> service)
        {
            return new Validator(service as ISnackService).Validate(this);
        }

        class Validator : AbstractValidator<SnackAddCommand>
        {
            public Validator(ISnackService service)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().Must((name) => !service.IsNameAlreadyInUse(name, 0)).WithMessage("O nome já está em uso"); ;
                RuleFor(c => c.Image).NotNull().NotEmpty().WithMessage("É necessário inserir uma imagem.");
            }
        }
    }
}
