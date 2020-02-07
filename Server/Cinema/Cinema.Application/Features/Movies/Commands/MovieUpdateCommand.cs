using Cinema.Domain.Common;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Movies.Commands
{
    public class MovieUpdateCommand : AbstractUpdateCommand<Movie>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Image { get; set; }
        public virtual int Length { get; set; }
        public virtual int Animation { get; set; }
        public virtual int Audio { get; set; }

        public override ValidationResult Validate(IService<Movie> service)
        {
            return new Validator(service as IMovieService).Validate(this);
        }

        class Validator : AbstractValidator<MovieUpdateCommand>
        {
            public Validator(IMovieService service)
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().Must((c, name) => !service.IsNameAlreadyInUse(name, c.Id)).WithMessage("O nome já está em uso");
                RuleFor(c => c.Length).GreaterThan(0);
                RuleFor(c => c.Image).NotNull().NotEmpty().WithMessage("É necessário inserir uma imagem.");
                RuleFor(c => c.Description).NotNull().NotEmpty();
                RuleFor(c => c.Id).GreaterThan(0);
            }
        }
    }
}
