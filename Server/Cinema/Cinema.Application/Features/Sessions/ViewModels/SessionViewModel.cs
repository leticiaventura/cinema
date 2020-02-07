using System;
using Cinema.Application.Features.Lounges.ViewModels;
using Cinema.Application.Features.Movies.ViewModels;

namespace Cinema.Application.Features.Sessions.ViewModels
{
    public class SessionViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Start { get; set; }
        public virtual MovieGridViewModel Movie { get; set; }
        public virtual LoungeViewModel Lounge { get; set; }
    }
}
