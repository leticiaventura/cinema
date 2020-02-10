using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinema.Application.Features.Purchases.Commands;
using Cinema.Domain.Features.Purchases;

namespace Cinema.Application.Features.Purchases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PurchaseAddCommand, Purchase>();
        }
    }
}
