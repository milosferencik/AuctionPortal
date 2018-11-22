using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Config
{
    public class MappingConfig
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Product, ProductDto>();
            config.CreateMap<ProductDto, Product>().ForMember(dest => dest.Category, opt => opt.Ignore());
            /*config.CreateMap<Category, CategoryDto>().ForMember(categoryDto => categoryDto.CategoryPath, opts => opts.ResolveUsing(category =>
            {
                var categoryPath = category.Name;
                while (category.Parent != null)
                {
                    categoryPath = category.Parent.Name + "/" + categoryPath;
                    category = category.Parent;
                }
                return categoryPath;
            })).ReverseMap();*/
            config.CreateMap<Auctioneer, AuctioneerDto>().ReverseMap();
            config.CreateMap<Bid, BidDto>().ReverseMap();
            config.CreateMap<Review, ReviewDto>().ReverseMap();
            config.CreateMap<User,UserCreateDto>().ReverseMap();
        
        }
    }
}
