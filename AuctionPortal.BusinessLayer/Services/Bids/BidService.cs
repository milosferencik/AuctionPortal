using System;
using System.Linq;
using System.Threading.Tasks;
using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.BusinessLayer.Services.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.Query;
using AutoMapper;

namespace AuctionPortal.BusinessLayer.Services.Bids
{
    public class BidService :  CrudQueryServiceBase<Bid, BidDto, BidFilterDto>, IBidService
    {
        public BidService(IMapper mapper, IRepository<Bid> bidRepository, QueryObjectBase<BidDto, Bid, BidFilterDto, IQuery<Bid>> bidQueryObject) 
            : base(mapper, bidRepository, bidQueryObject) { }

        protected override async Task<Bid> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        public override Guid Create(BidDto entityDto)
        {
            var price = GetLastBidForProduct(entityDto.ProductId)?.Result.Price ?? entityDto.Product.StartPrice;
            if (price + entityDto.Product.MinimalBid > entityDto.Price)
            {
                throw new ArgumentException("Bid is to small!");
            }
            if (entityDto.Bidder.Money < entityDto.Price)
            {
                throw new ArgumentException("Don't enough money!");
            }
            var entity = Mapper.Map<Bid>(entityDto);
            Repository.Create(entity);
            return entity.Id;

        }

        public async Task<BidDto> GetLastBidForProduct(Guid productId)
        {
            var queryResult = await Query.ExecuteQuery(new BidFilterDto { ProductId = productId });
            return queryResult.Items.OrderByDescending(bid => bid.Price).FirstOrDefault();
        }
    }
}
