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
        private readonly IRepository<Product> productsRepository;

        public AuctioneerService(IMapper mapper, IRepository<Auctioneer> auctioneerRepository, QueryObjectBase<AuctioneerDto, Auctioneer, AuctioneerFilterDto, IQuery<Auctioneer>> auctioneerQueryObject, IRepository<Product> productsRepository)
            : base(mapper, auctioneerRepository, auctioneerQueryObject)
        {
            this.productsRepository = productsRepository;
        }

        protected override async Task<Auctioneer> GetWithIncludesAsync(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        public async Task<Auctioneer> GetAuctioneerEntity(Guid entityId)
        {
            return await Repository.GetAsync(entityId);
        }

        /// <summary>
        /// Gets customer with given email address
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>Customer with given email address</returns>
        public async Task<AuctioneerDto> GetAuctioneerAccordingToEmailAsync(string email)
        {
            var queryResult = await Query.ExecuteQuery(new AuctioneerFilterDto { Email = email });
            return queryResult.Items.SingleOrDefault();
        }

        public async Task<AuctioneerDto> GetAuctioneerDtoAsync(Guid id)
        {
            var auctioneer = await GetAuctioneerEntity(id);
            return Mapper.Map<AuctioneerDto>(auctioneer);
        }
    }
}
