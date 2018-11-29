using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.Facades.Common;
using AuctionPortal.BusinessLayer.Services.Bids;
using AuctionPortal.BusinessLayer.Services.Categories;
using AuctionPortal.BusinessLayer.Services.Products;
using AuctionPortal.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Facades
{
    public class ProductFacade : FacadeBase
    {
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly IBidService bidService;

        public ProductFacade(IUnitOfWorkProvider unitOfWorkProvider, ICategoryService categoryService, IProductService productService, IBidService bidService) : base(unitOfWorkProvider)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.bidService = bidService;
        }

        /// <summary>
        /// get product according to id
        /// </summary>
        /// <param name="id"> product id</param>
        /// <returns> product</returns>
        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var product = await productService.GetAsync(id);
                return product;
            }
        }

        /// <summary>
        /// get product according to name
        /// </summary>
        /// <param name="name">product name </param>
        /// <returns> product</returns>
        public async Task<ProductDto> GetProductAsync(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                var product = await productService.GetProductByNameAsync(name);
                return product;
            }
        }

        /// <summary>
        /// Gets products according to filter and required page
        /// </summary>
        /// <param name="filter">products filter</param>
        /// <returns></returns>
        public async Task<QueryResultDto<ProductDto, ProductFilterDto>> GetProductsAsync(ProductFilterDto filter)
        {
            using (UnitOfWorkProvider.Create())
            {
                if (filter.CategoryIds == null && filter.CategoryNames != null)
                {
                    filter.CategoryIds = await categoryService.GetCategoryIdsByNamesAsync(filter.CategoryNames);
                }
                var productListQueryResult = await productService.ListProductsAsync(filter);
                return productListQueryResult;
            }
        }

        /// <summary>
        /// Creates product with category that corresponds with given name
        /// </summary>
        /// <param name="product">product</param>
        /// <param name="categoryName">category name</param>
        public async Task<Guid> CreateProductWithCategoryNameAsync(ProductDto product, string categoryName)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                product.CategoryId = (await categoryService.GetCategoryIdsByNamesAsync(categoryName)).FirstOrDefault();
                var productId = productService.Create(product);
                await uow.Commit();
                return productId;
            }
        }

        /// <summary>
        /// Updates product
        /// </summary>
        /// <param name="productDto">Product details</param>
        public async Task<bool> EditProductAsync(ProductDto productDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if ((await productService.GetAsync(productDto.Id, false)) == null)
                {
                    return false;
                }
                await productService.Update(productDto);
                await uow.Commit();
                return true;
            }
        }

        //delete?

        /// <summary>
        /// Gets category according to ID
        /// </summary>
        /// <param name="categoryId">category ID</param>
        /// <returns>The category</returns>
        public async Task<CategoryDto> GetCategoryAsync(Guid categoryId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await categoryService.GetAsync(categoryId);
            }
        }

        /// <summary>
        /// Gets ids of the categories with the corresponding names
        /// </summary>
        /// <param name="names">names of the categories</param>
        /// <returns>ids of categories with specified name</returns>
        public async Task<Guid[]> GetProductCategoryIdsByNamesAsync(params string[] names)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await categoryService.GetCategoryIdsByNamesAsync(names);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <returns>All available categories</returns>
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            using (UnitOfWorkProvider.Create())
            {
                return (await categoryService.ListAllAsync()).Items;
            }
        }

        /// <summary>
        /// gets current price for product
        /// </summary>
        /// <param name="id"> product id</param>
        /// <returns> current price for product</returns>
        public async Task<decimal> GetCurrentPriceForProduct(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                var prod = await bidService.GetLastBidForProduct(id);
                return prod.Price;
            }
        }
    }
}
