using System;
using System.Collections.ObjectModel;
using Cinema.Domain.Common;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Purchases;

namespace Cinema.Domain.Features.Sessions
{
    public class Session : Entity
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual Movie Movie { get; set; }
        public long MovieId { get; set; }
        public virtual Lounge Lounge { get; set; }
        public long LoungeId { get; set; }
        public double Price { get; set; }
        public int PurchasedSeats { get; set; }
        public Collection<Purchase> Purchases { get; set; }

        public override void Validate()
        {
            if (End != Start.AddMinutes(Movie.Length))
                throw new BusinessException(ErrorCodes.BadRequest, "O horário de fim deve ser igual ao início + duração do filme.");
            if (PurchasedSeats > Lounge.Seats)
                throw new BusinessException(ErrorCodes.BadRequest, "A quantidade de lugares compradas não pode ser maior que a quantidade de lugares disponíveis na sala.");
        }
    }
}
