using AuctionPortal.BusinessLayer.DataTransferObjects;
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

namespace AuctionPortal.BusinessLayer.Services.Reviews
{
    public class ReviewService : CrudQueryServiceBase<Review, ReviewDto, ReviewFilterDto>, IReviewService
    {
        public ReviewService(IMapper mapper, IRepository<Review> reviewRepository, QueryObjectBase<ReviewDto, Review, ReviewFilterDto, IQuery<Review>> reviewQuery)
            : base(mapper, reviewRepository, reviewQuery) { }

        public async Task<IList<ReviewDto>> GetAllReviewsForUser(Guid reviewedId, bool withIncludes = true)
        {
            var queryResult = await Query.ExecuteQuery(new ReviewFilterDto { ReviewedId = reviewedId });
            return queryResult.Items.OrderByDescending(x => x.Rating).ToList();
        }

        protected override async Task<Review> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        public async Task<Review> GetReviewEntity(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        public async Task DeleteAllReviewsForUser(Guid userId)
        {
            var reviews = await GetAllReviewsForUser(userId);
            foreach(var review in reviews)
            {
                DeleteProduct(review.Id);
            }
        }
    }
}
