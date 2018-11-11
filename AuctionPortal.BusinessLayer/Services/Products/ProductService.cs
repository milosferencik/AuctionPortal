using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.BusinessLayer.Services.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.Query;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Products
{
    public class ProductService : CrudQueryServiceBase<Product, ProductDto, ProductFilterDto>, IProductService
    {
        public ProductService(IMapper mapper, QueryObjectBase<ProductDto, Product, ProductFilterDto, IQuery<Product>> productQuery, IRepository<Product> productRepository)
            : base(mapper, productRepository, productQuery) { }

        protected override Task<Product> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId, nameof(Product.Category));
        }

        /// <summary>
        /// Gets product with given name
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product with given name</returns>
        public async Task<ProductDto> GetProductByNameAsync(string name)
        {
            var queryResult = await Query.ExecuteQuery(new ProductFilterDto { SearchedName = name });
            return queryResult.Items.SingleOrDefault();
        }

        /// <summary>
        /// Gets products according to given filter
        /// </summary>
        /// <param name="filter">The products filter</param>
        /// <returns>Filtered results</returns>
        public async Task<QueryResultDto<ProductDto, ProductFilterDto>> ListProductsAsync(ProductFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }

        public async Task<IList<ProductDto>> GetAllProductsWithGivenSellerId(Guid SellerId)
        {
            var queryResult = await Query.ExecuteQuery(new ProductFilterDto { SellerId = SellerId });
            return queryResult.Items.Where(x => x.ValidTo >= DateTime.Now).ToList();
        }
    }
}
