using AutoMapper;
using RavenDBtoSqlServerDataLoad.SQLServerEntities;
using Microsoft.Extensions.DependencyInjection;

namespace RavenDBtoSqlServerDataLoad.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<RavenDBEntities.Customer, Customer>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.Orders, opt => opt.Ignore());

            //CreateMap<RavenDBEntities.Order, Order>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
            //    .ForMember(dest => dest.Customer, opt => opt.Ignore())
            //    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            //CreateMap<RavenDBEntities.OrderItem, OrderItem>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            //    .ForMember(dest => dest.Order, opt => opt.Ignore());


            CreateMap<RavenDBEntities.Customer, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id));

            CreateMap<RavenDBEntities.Order, Order>();
                //.ForMember(dest => dest.Id, opt => opt.Ignore())
                //.ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId));

            CreateMap<RavenDBEntities.OrderItem, OrderItem>();

        }
    }
}
