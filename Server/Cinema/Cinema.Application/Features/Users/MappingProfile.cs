using AutoMapper;
using Cinema.Application.Features.Users.Commands;
using Cinema.Application.Features.Users.ViewModels;
using Cinema.Domain.Features.Users;
using Cinema.Infra.Crypto;

namespace Cinema.Application.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserAddCommand, User>()
                .ForMember(d => d.Password, o => o.MapFrom(value => value.Password.GenerateHash()))
                .ForMember(d => d.Permission, o => o.MapFrom(value => (EnumPermissionLevel)value.PermissionLevel));
            CreateMap<UserUpdateCommand, User>()
                .ForMember(d => d.Password, o => o.MapFrom(value => value.Password.GenerateHash()))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, UserViewModel>().ForMember(d => d.PermissionLevel, o => o.MapFrom(value => value.Permission.GetHashCode()));
        }
    }
}
