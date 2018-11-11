using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure.Query;
using AuctionPortal.Infrastructure.Query.Predicates;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuctionPortal.BusinessLayer.QueryObjects
{
    public class BidQueryObject : QueryObjectBase<BidDto, Bid, BidFilterDto, IQuery<Bid>>
    {
        public BidQueryObject(IMapper mapper, IQuery<Bid> query) : base(mapper, query) { }

        protected override IQuery<Bid> ApplyWhereClause(IQuery<Bid> query, BidFilterDto filter)
        {
            var definedPredicates = new List<IPredicate>();
            AddIfDefined(FilterProductId(filter), definedPredicates);
            AddIfDefined(FilterAuctioneerId(filter), definedPredicates);
            if (definedPredicates.Count == 0)
            {
                return query;
            }
            if (definedPredicates.Count == 1)
            {
                return query.Where(definedPredicates.First());
            }
            var wherePredicate = new CompositePredicate(definedPredicates);
            return query.Where(wherePredicate);
        }

        private static void AddIfDefined(IPredicate categoryPredicate, ICollection<IPredicate> definedPredicates)
        {
            if (categoryPredicate != null)
            {
                definedPredicates.Add(categoryPredicate);
            }
        }

        private static SimplePredicate FilterProductId(BidFilterDto filter)
        {
            if (filter.ProductId.Equals(Guid.Empty))
            {
                return null;
            }
            return new SimplePredicate(nameof(Bid.ProductId), Infrastructure.Query.Predicates.Operators.ValueComparingOperator.Equal, filter.ProductId);
        }

        private static SimplePredicate FilterAuctioneerId(BidFilterDto filter)
        {
            if (filter.AuctioneerId.Equals(Guid.Empty))
            {
                return null;
            }
            return new SimplePredicate(nameof(Bid.BidderId), Infrastructure.Query.Predicates.Operators.ValueComparingOperator.Equal, filter.AuctioneerId);
        }
    }
}
