using System;
using System.Collections.ObjectModel;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Sessions;

namespace Cinema.Domain.Features.Lounges
{
    public class Lounge : Entity
    {
        public String Name { get; set; }
        public int Seats { get; set; }
        public Collection<Session> Sessions { get; set; }
    }
}
