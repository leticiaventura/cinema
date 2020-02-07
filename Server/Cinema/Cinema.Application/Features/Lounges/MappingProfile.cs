using AutoMapper;
using Cinema.Application.Features.Lounges.Commands;
using Cinema.Application.Features.Lounges.ViewModels;
using Cinema.Domain.Features.Lounges;

namespace Cinema.Application.Features.Lounges
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoungeAddCommand, Lounge>();
            CreateMap<LoungeUpdateCommand, Lounge>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Lounge, LoungeViewModel>();
        }
    }
}
