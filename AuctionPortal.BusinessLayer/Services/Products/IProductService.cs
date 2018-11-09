using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Products
{
    public interface IProductService
    {
        /// <summary>
        /// Gets products according to given filter
        /// </summary>
        /// <param name="filter">The products filter</param>
        /// <returns>Filtered results</returns>
        Task<QueryResultDto<ProductDto, ProductFilterDto>> ListProductsAsync(ProductFilterDto filter);

        /// <summary>
        /// Gets DTO representing the entity according to ID
        /// </summary>
        /// <param name="entityId">entity ID</param>
        /// <param name="withIncludes">include all entity complex types</param>
        /// <returns>The DTO representing the entity</returns>
        Task<ProductDto> GetAsync(Guid entityId, bool withIncludes = true);

        /// <summary>
        /// Gets product with given name
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product with given name</returns>
        Task<ProductDto> GetProductByNameAsync(string name);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Guid Create(ProductDto entityDto);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Task Update(ProductDto entityDto);

        /// <summary>
        /// Deletes entity with given Id
        /// </summary>
        /// <param name="entityId">Id of the entity to delete</param>
        void DeleteProduct(Guid entityId);

        /// <summary>
        /// Gets all DTOs (for given type)
        /// </summary>
        /// <returns>all available dtos (for given type)</returns>
        Task<QueryResultDto<ProductDto, ProductFilterDto>> ListAllAsync();
    }
}
