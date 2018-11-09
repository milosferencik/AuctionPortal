using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.QueryObjects.Common;
using AuctionPortal.BusinessLayer.Services.Common;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.Query;
using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Services.Auctioneers
{
    public class AuctioneerService : CrudQueryServiceBase<Auctioneer, AuctioneerDto, AuctioneerFilterDto>, IAuctioneerService
    {
        public AuctioneerService(IMapper mapper, IRepository<Auctioneer> auctioneerRepository, QueryObjectBase<AuctioneerDto, Auctioneer, AuctioneerFilterDto, IQuery<Auctioneer>> auctioneerQueryObject)
            : base(mapper, auctioneerRepository, auctioneerQueryObject) { }

        protected override async Task<Auctioneer> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        /// <summary>
        /// Gets customer with given email address
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Customer with given email address</returns>
        public async Task<AuctioneerDto> GetCustomerAccordingToEmailAsync(string email)
        {
            var queryResult = await Query.ExecuteQuery(new AuctioneerFilterDto { Email = email });
            return queryResult.Items.SingleOrDefault();
        }
    }
}
