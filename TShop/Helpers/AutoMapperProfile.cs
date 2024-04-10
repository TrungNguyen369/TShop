using AutoMapper;
using TShop.Models;
using TShop.Services;
using TShop.ViewModels;

namespace TShop.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductVM, Product>();
            CreateMap<Product, ProductVM>();
            CreateMap<UserVM, Customer>();
            CreateMap<Customer, UserVM>();
        }
    }
}
