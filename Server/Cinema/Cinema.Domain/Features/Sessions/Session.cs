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
        public virtual Collection<Purchase> Purchases { get; set; }
        public virtual Collection<Seat> TakenSeats { get; set; }

        public override void Validate()
        {
            if (End != Start.AddMinutes(Movie.Length))
                throw new BusinessException(ErrorCodes.BadRequest, "O horário de fim deve ser igual ao início + duração do filme.");
        }
    }
}
