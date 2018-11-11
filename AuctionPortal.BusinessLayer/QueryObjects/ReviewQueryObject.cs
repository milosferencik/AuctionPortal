using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure.Query;
using AuctionPortal.Infrastructure.Query.Predicates;
using AutoMapper;
using System;

namespace AuctionPortal.BusinessLayer.QueryObjects
{
    public class ReviewQueryObject : QueryObjectBase<ReviewDto, Review, ReviewFilterDto, IQuery<Review>>
    {
        public ReviewQueryObject(IMapper mapper, IQuery<Review> query) : base(mapper, query) { }

        protected override IQuery<Review> ApplyWhereClause(IQuery<Review> query, ReviewFilterDto filter)
        {
            if (filter.ReviewedId.Equals(Guid.Empty))
            {
                return query;
            }
            return query.Where(new SimplePredicate(nameof(Review.ReviewedId), Infrastructure.Query.Predicates.Operators.ValueComparingOperator.Equal, filter.ReviewedId));
        }
    }
}
