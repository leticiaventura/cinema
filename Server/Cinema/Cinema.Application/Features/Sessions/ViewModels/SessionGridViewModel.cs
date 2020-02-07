using System;

namespace Cinema.Application.Features.Sessions.ViewModels
{
    public class SessionGridViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Start { get; set; }
        public virtual string End { get; set; }
        public virtual string Movie { get; set; }
        public virtual string Lounge { get; set; }
        public virtual double Price { get; set; }
        public virtual int Animation { get; set; }
        public virtual int Audio { get; set; }
        public virtual int FreeSeats { get; set; }
    }
}
