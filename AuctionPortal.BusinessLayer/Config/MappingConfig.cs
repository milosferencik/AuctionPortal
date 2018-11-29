using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure.Query;
using AutoMapper;

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
            config.CreateMap<Auctioneer, UserCreateDto>().ReverseMap();
            config.CreateMap<User, UserDto>().ReverseMap();

            config.CreateMap<QueryResult<Product>, QueryResultDto<ProductDto, ProductFilterDto>>();
            config.CreateMap<QueryResult<Category>, QueryResultDto<CategoryDto, CategoryFilterDto>>();
            config.CreateMap<QueryResult<User>, QueryResultDto<UserDto, UserFilterDto>>();
            config.CreateMap<QueryResult<Auctioneer>, QueryResultDto<AuctioneerDto, AuctioneerFilterDto>>();
            config.CreateMap<QueryResult<Bid>, QueryResultDto<BidDto, BidFilterDto>>();

        }
    }
}
