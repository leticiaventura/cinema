using System.Collections.Generic;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Purchases.Interfaces;
using Cinema.Domain.Features.Snacks;
using FluentValidation;
using FluentValidation.Results;

namespace Cinema.Application.Features.Purchases.Commands
{
    public class PurchaseAddCommand : AbstractAddCommand<Purchase>
    {
        public virtual long SessionId { get; set; }
        public virtual ICollection<PurchaseSnack> SnacksArray { get; set; }
        public virtual ICollection<Seat> SeatsArray { get; set; }
        public virtual string UserEmail { get; set; }

        public override ValidationResult Validate(IService<Purchase> service)
        {
            return new Validator(service as IPurchaseService).Validate(this);
        }

        class Validator : AbstractValidator<PurchaseAddCommand>
        {
            public Validator(IPurchaseService service)
            {
                RuleFor(c => c.SessionId).GreaterThan(0);
                RuleFor(x => x.SeatsArray.Count).GreaterThan(0);
            }
        }
    }
}
