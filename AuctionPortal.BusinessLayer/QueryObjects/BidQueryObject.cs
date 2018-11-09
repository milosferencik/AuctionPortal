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
    public class BidQueryObject : QueryObjectBase<BidDto, Bid, BidFilterDto, IQuery<Bid>>
    {
        public BidQueryObject(IMapper mapper, IQuery<Bid> query) : base(mapper, query) { }

        protected override IQuery<Bid> ApplyWhereClause(IQuery<Bid> query, BidFilterDto filter)
        {
            if (filter.ProductId.Equals(Guid.Empty))
            {
                return query;
            }
            return query.Where(new SimplePredicate(nameof(Bid.ProductId), Infrastructure.Query.Predicates.Operators.ValueComparingOperator.Equal, filter.ProductId));
        }
    }
}
