using System;
using AutoMapper;
using Cinema.Application.Features.Sessions.Commands;
using Cinema.Application.Features.Sessions.Queries;
using Cinema.Application.Features.Sessions.ViewModels;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Sessions;

namespace Cinema.Application.Features.Sessions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SessionAddCommand, Session>();

            CreateMap<SessionGetAvailableLoungesQuery, Session>()
                .ForMember(d => d.Start, o => o.MapFrom(value => Convert.ToDateTime(value.Start)))
                .ForMember(d => d.End, o => o.MapFrom(value => Convert.ToDateTime(value.Start).AddMinutes(value.MovieLength)));

            CreateMap<Session, SessionViewModel>()
                .ForMember(d => d.Start, o => o.MapFrom(value => value.Start.ToString("o")));

            CreateMap<Session, SessionGridViewModel>()
                .ForMember(d => d.Movie, o => o.MapFrom(value => value.Movie.Name))
                .ForMember(d => d.Lounge, o => o.MapFrom(value => value.Lounge.Name))
                .ForMember(d => d.Audio, o => o.MapFrom(value => value.Movie.Audio.GetHashCode()))
                .ForMember(d => d.Animation, o => o.MapFrom(value => value.Movie.Animation.GetHashCode()))
                .ForMember(d => d.FreeSeats, o => o.MapFrom(value => value.Lounge.Seats - value.PurchasedSeats))
                .ForMember(d => d.Start, o => o.MapFrom(value => value.Start.ToString("o")))
                .ForMember(d => d.End, o => o.MapFrom(value => value.End.ToString("o")));
        }
    }
}
