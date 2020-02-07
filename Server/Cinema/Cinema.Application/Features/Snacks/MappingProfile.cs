using System;
using AutoMapper;
using Cinema.Application.Features.Snacks.Commands;
using Cinema.Application.Features.Snacks.ViewModels;
using Cinema.Domain.Features.Snacks;

namespace Cinema.Application.Features.Snacks
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SnackAddCommand, Snack>()
                .ForMember(d => d.Image, o => o.MapFrom(value => Convert.FromBase64String(value.Image.Replace("data:image/jpeg;base64,", ""))));
            CreateMap<SnackUpdateCommand, Snack>()
                .ForMember(d => d.Image, o => o.MapFrom(value => Convert.FromBase64String(value.Image.Replace("data:image/jpeg;base64,", ""))))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Snack, SnackViewModel>()
                .ForMember(d => d.Image, o => o.MapFrom(value => "data:image/jpeg;base64," + Convert.ToBase64String(value.Image)));
            CreateMap<Snack, SnackGridViewModel>();
        }
    }
}
