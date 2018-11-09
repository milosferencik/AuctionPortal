using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure.Query;
using AuctionPortal.Infrastructure.Query.Predicates;
using AuctionPortal.Infrastructure.Query.Predicates.Operators;
using AutoMapper;

namespace AuctionPortal.BusinessLayer.QueryObjects
{
    public class AuctioneerQueryObject : QueryObjectBase<AuctioneerDto, Auctioneer, AuctioneerFilterDto, IQuery<Auctioneer>>
    {
        public AuctioneerQueryObject(IMapper mapper, IQuery<Auctioneer> query) : base(mapper, query) { }

        protected override IQuery<Auctioneer> ApplyWhereClause(IQuery<Auctioneer> query, AuctioneerFilterDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Email))
            {
                return query;
            }
            return query.Where(new SimplePredicate(nameof(Auctioneer.Email), ValueComparingOperator.Equal, filter.Email));
        }
    }
}
