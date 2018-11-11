using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.Facades.Common;
using AuctionPortal.BusinessLayer.Services.Auctioneers;
using AuctionPortal.BusinessLayer.Services.Bids;
using AuctionPortal.BusinessLayer.Services.Products;
using AuctionPortal.BusinessLayer.Services.Reviews;
using AuctionPortal.BusinessLayer.Services.Users;
using AuctionPortal.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Facades
{
    class AuctioneerFacade : FacadeBase
    {
        private readonly IAuctioneerService auctioneerService;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly IReviewService reviewService;
        private readonly IBidService bidService;

        protected AuctioneerFacade(IUnitOfWorkProvider unitOfWorkProvider, IAuctioneerService auctioneerService, IProductService productService
            , IUserService userService, IReviewService reviewService, IBidService bidService) : base(unitOfWorkProvider)
        {
            this.auctioneerService = auctioneerService;
            this.productService = productService;
            this.userService = userService;
            this.reviewService = reviewService;
            this.bidService = bidService;
        }

        /// <summary>
        /// get auctioneer by email address
        /// </summary>
        /// <param name="email">email address</param>
        /// <returns>auctioneer with email address</returns>
        public async Task<AuctioneerDto> GetAuctioneerAccordingToEmailAsync(string email)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await auctioneerService.GetAuctioneerAccordingToEmailAsync(email);
            }
        }

        /// <summary>
        /// Gets all auctioneers according to page
        /// </summary>
        /// <returns>all auctioneers</returns>
        public async Task<QueryResultDto<AuctioneerDto, AuctioneerFilterDto>> GetAllAuctioneersAsync()
        {
            using (UnitOfWorkProvider.Create())
            {
                return await auctioneerService.ListAllAsync();
            }
        }

        /// <summary>
        /// creates new user and auctioneer
        /// </summary>
        /// <param name="registrationDto"> new user dto </param>
        /// <param name="success">is successful</param>
        /// <returns>if operation is successful</returns>
        public Guid RegisterUser(UserCreateDto registrationDto, out bool success)
        {
            if(auctioneerService.GetAuctioneerAccordingToEmailAsync(registrationDto.Auctioneer.Email) != null)
            {
                success = false;
                return new Guid();
            }
            userService.Create(registrationDto); 
            var auctioneerId = auctioneerService.Create(registrationDto.Auctioneer);
            success = true;
            return auctioneerId;
        }

        /// <summary>
        /// deletes user
        /// </summary>
        /// <param name="auctioneerId"> user to be deleted </param>
        /// <returns>whether user was deleted</returns>
        public async Task<bool> Delete(Guid auctioneerId)
        {
            if (productService.GetAllProductsWithGivenSellerId(auctioneerId) != null)
            {
                return false;
            }
            reviewService.DeleteAllReviewsForUser(auctioneerId);
            var auctioneer = await auctioneerService.GetAuctioneerEntity(auctioneerId);
            userService.DeleteProduct(auctioneer.User.Id);
            auctioneerService.DeleteProduct(auctioneerId);
            return true;
        }

        public async void AddReview(ReviewDto review)
        {
            reviewService.Create(review);
            review.Reviewed.Rating = await CalculateRating(review.ReviewedId);
            await auctioneerService.Update(review.Reviewed);
        }

        public async void DeleteReview(Guid ReviewId)
        {
            var review = await reviewService.GetReviewEntity(ReviewId);
            var reviewed = await auctioneerService.GetAsync(review.ReviewedId);
            reviewService.DeleteProduct(ReviewId);
            reviewed.Rating = await CalculateRating(reviewed.Id);
            await auctioneerService.Update(reviewed);
        }

        private async Task<double> CalculateRating(Guid auctioneerId)
        {
            var reviews = await reviewService.GetAllReviewsForUser(auctioneerId);
            return reviews.Sum(x => x.Rating) / reviews.Count;
        }

        public async Task<IList<ProductDto>> GetAllActiveProductsUserPutBidsOn(Guid auctioneerId)
        {
            var bids = await bidService.GetAllBidsForUser(auctioneerId);
            return  bids.Where(x => x.Product.ValidTo >= DateTime.Now).Select(z => z.Product).Distinct().ToList();
        }
    }
}
