using AuctionPortal.BusinessLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Bids
{
    public interface IBidService
    {
        /// <summary>
        /// Gets bid with given id
        /// </summary>
        /// <param name="entityId">bid id</param>
        /// <returns>BidDto with given id</returns>
        Task<BidDto> GetAsync(Guid entityId, bool withIncludes = true);

        /// <summary>
        /// Get last bid
        /// </summary>
        /// <param name="productId">product id</param>
        /// <returns> Last bid for product</returns>
        Task<BidDto> GetLastBidForProduct(Guid productId);

        Task<IOrderedEnumerable<BidDto>> GetBidsForProductOrdered(Guid productId);

        Task<IList<BidDto>> GetAllBidsForUser(Guid userId);

        /// <summary>
        /// Creates new bid
        /// </summary>
        /// <param name="entityDto">entity details</param>
        Guid CreateBid(BidDto entityDto);

        Task DeleteAllBidsForProduct(Guid productId);
    }
}
