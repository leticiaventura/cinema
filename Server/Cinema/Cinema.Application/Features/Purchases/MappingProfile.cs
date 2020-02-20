using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinema.Application.Features.Purchases.Commands;
using Cinema.Application.Features.Purchases.ViewModels;
using Cinema.Domain.Features.Purchases;

namespace Cinema.Application.Features.Purchases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PurchaseAddCommand, Purchase>();

            CreateMap<Purchase, PurchaseGridViewModel>()
                .ForMember(d => d.Movie, o => o.MapFrom(value => value.MovieName))
                .ForMember(d => d.Lounge, o => o.MapFrom(value => value.Session.Lounge.Name))
                .ForMember(d => d.Date, o => o.MapFrom(value => value.SessionDate));
        }
    }
}
