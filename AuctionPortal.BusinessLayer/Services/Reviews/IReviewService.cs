using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Reviews
{
    public interface IReviewService
    {
        /// <summary>
        /// get review
        /// </summary>
        /// <param name="entityId">review id</param>
        /// <returns>reviewdto</returns>
        Task<IList<ReviewDto>> GetAllReviewsForUser(Guid entityId, bool withIncludes = true);

        /// <summary>
        /// Creates new entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Guid Create(ReviewDto entityDto);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Task Update(ReviewDto entityDto);

        /// <summary>
        /// Deletes entity with given Id
        /// </summary>
        /// <param name="entityId">Id of the entity to delete</param>
        void DeleteProduct(Guid entityId);

        Task<Review> GetReviewEntity(Guid entityId);

        Task DeleteAllReviewsForUser(Guid userId);

        Task<QueryResultDto<ReviewDto, ReviewFilterDto>> ListAllAsync();

    }
}
