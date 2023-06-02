using API.DTOS;
using AutoMapper;
using Core.DTOS.Auth;
using Core.DTOS.BasketItems;
using Core.Entities;
using Infrastructure.Extend;

namespace Core.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s=>s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s=>s.ProductType.Name));

            CreateMap<BasktItem, BasketToReturnDto>().ReverseMap();
        }
    }
}
