using System;
using AutoMapper;
using Cinema.Application.Features.Movies.Commands;
using Cinema.Application.Features.Movies.ViewModels;
using Cinema.Domain.Features.Movies;

namespace Cinema.Application.Features.Movies
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieAddCommand, Movie>()
                .ForMember(d => d.Image, o => o.MapFrom(value => Convert.FromBase64String(value.Image.Replace("data:image/jpeg;base64,", ""))))
                .ForMember(d => d.Animation, o => o.MapFrom(value => (EnumAnimation)value.Animation))
                .ForMember(d => d.Audio, o => o.MapFrom(value => (EnumAnimation)value.Audio));
            CreateMap<MovieUpdateCommand, Movie>()
                .ForMember(d => d.Image, o => o.MapFrom(value => Convert.FromBase64String(value.Image.Replace("data:image/jpeg;base64,", ""))))
                .ForMember(d => d.Animation, o => o.MapFrom(value => (EnumAnimation)value.Animation))
                .ForMember(d => d.Audio, o => o.MapFrom(value => (EnumAnimation)value.Audio))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Movie, MovieViewModel>()
                .ForMember(d => d.Image, o => o.MapFrom(value => "data:image/jpeg;base64," + Convert.ToBase64String(value.Image)))
                .ForMember(d => d.Animation, o => o.MapFrom(value => value.Animation.GetHashCode()))
                .ForMember(d => d.Audio, o => o.MapFrom(value => value.Audio.GetHashCode())); 
            CreateMap<Movie, MovieGridViewModel>()
                .ForMember(d => d.Animation, o => o.MapFrom(value => value.Animation.GetHashCode()))
                .ForMember(d => d.Audio, o => o.MapFrom(value => value.Audio.GetHashCode()));
        }
    }
}
