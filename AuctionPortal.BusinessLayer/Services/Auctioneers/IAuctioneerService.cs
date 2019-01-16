using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using System;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Auctioneers
{
    public interface IAuctioneerService
    {
        /// <summary>
        /// Gets auctioneer with given email address
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Auctioneer with given email address</returns>
        Task<AuctioneerDto> GetAuctioneerAccordingToEmailAsync(string email);

        /// <summary>
        /// Gets DTO representing the entity according to ID
        /// </summary>
        /// <param name="entityId">entity ID</param>
        /// <param name="withIncludes">include all entity complex types</param>
        /// <returns>The DTO representing the entity</returns>
        Task<AuctioneerDto> GetAsync(Guid entityId, bool withIncludes = true);

        /// <summary>
        /// get auctioneer with id
        /// </summary>
        /// <param name="entityId">auctioneer id</param>
        /// <returns>auctioneer with id</returns>
        Task<Auctioneer> GetAuctioneerEntity(Guid entityId);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Guid Create(AuctioneerDto entityDto);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Task Update(AuctioneerDto entityDto);

        /// <summary>
        /// Deletes entity with given Id
        /// </summary>
        /// <param name="entityId">Id of the entity to delete</param>
        void DeleteProduct(Guid entityId);

        /// <summary>
        /// Gets all DTOs (for given type)
        /// </summary>
        /// <returns>all available dtos (for given type)</returns>
        Task<QueryResultDto<AuctioneerDto, AuctioneerFilterDto>> ListAllAsync();

        /// <summary>
        /// Gets auctioneers according to given filter
        /// </summary>
        /// <param name="filter">The products filter</param>
        /// <returns>Filtered results</returns>
        Task<QueryResultDto<AuctioneerDto, AuctioneerFilterDto>> ListAuctioneerAsync(AuctioneerFilterDto filter);

        Task<AuctioneerDto> GetAuctioneerDtoAsync(Guid id);
    }
}
