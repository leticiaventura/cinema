using System;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Sessions.Commands
{
    public class SessionAddCommand : AbstractAddCommand<Session>
    {
        public virtual DateTime Start { get; set; }
        public virtual long MovieId{ get; set; }
        public virtual long LoungeId { get; set; }
        public virtual double Price { get; set; }

        public override ValidationResult Validate(IService<Session> service)
        {
            return new Validator(service as ISessionService).Validate(this);
        }

        class Validator : AbstractValidator<SessionAddCommand>
        {
            public Validator(ISessionService service)
            {
                RuleFor(c => c.Start).NotNull().NotEmpty().GreaterThan(DateTime.Now).WithMessage("A data deve ser superior à data atual.");
                RuleFor(c => c.MovieId).GreaterThan(0);
                RuleFor(c => c.LoungeId).GreaterThan(0);
            }
        }
    }
}
