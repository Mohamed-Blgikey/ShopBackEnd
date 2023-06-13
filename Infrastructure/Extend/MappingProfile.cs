using API.DTOS;
using AutoMapper;
using Core.DTOS;
using Core.DTOS.Auth;
using Core.DTOS.BasketItems;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure.DTOS;
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
            
            CreateMap<AddressDTo,OrderAddress>().ReverseMap();
            CreateMap<AddressDTo,Address>().ReverseMap();



            CreateMap<Order, OrderToReturnDto>();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                ;
        }
    }
}
