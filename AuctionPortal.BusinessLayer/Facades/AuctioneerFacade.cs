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
using System.Threading.Tasks;

namespace AuctionPortal.BusinessLayer.Facades
{
    public class AuctioneerFacade : FacadeBase
    {
        private readonly IAuctioneerService auctioneerService;
        private readonly IUserService userService;
        private readonly IProductService productService;
        private readonly IReviewService reviewService;
        private readonly IBidService bidService;

        public AuctioneerFacade(IUnitOfWorkProvider unitOfWorkProvider, IAuctioneerService auctioneerService, IProductService productService
            , IUserService userService, IReviewService reviewService, IBidService bidService) : base(unitOfWorkProvider)
        {
            this.auctioneerService = auctioneerService;
            this.productService = productService;
            this.userService = userService;
            this.reviewService = reviewService;
            this.bidService = bidService;
        }

        public async Task<AuctioneerDto> GetAuctioneerAsync(Guid id)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await auctioneerService.GetAuctioneerDtoAsync(id);
            }
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
        public async Task<Guid> RegisterUser(UserCreateDto registrationDto)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var id = await userService.RegisterUser(registrationDto);
                await uow.Commit();
                return id;
            }
        }

        /// <summary>
        /// login user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(bool success, bool isAdmin)> Login(string username, string password)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.AuthorizeUserAsync(username, password);
            }
        }

        public async Task<UserDto> GetUserAccordingToUsernameAsync(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                return await userService.GetAsync(username);
            }
        }

        public async Task<AuctioneerDto> GetAuctioneerAccordingToUsernameAsync(string username)
        {
            using (UnitOfWorkProvider.Create())
            {
                var user = await userService.GetAsync(username);
                return await auctioneerService.GetAsync(user.Id);
            }
        }

        /// <summary>
        /// deletes user
        /// </summary>
        /// <param name="auctioneerId"> user to be deleted </param>
        /// <returns>whether user was deleted</returns>
        public async Task<bool> Delete(Guid auctioneerId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                if (productService.GetAllProductsWithGivenSellerId(auctioneerId) != null)
                {
                    return false;
                }
                reviewService.DeleteAllReviewsForUser(auctioneerId);
                var auctioneer = await auctioneerService.GetAuctioneerEntity(auctioneerId);
                userService.DeleteUser(auctioneer.Id);
                auctioneerService.DeleteProduct(auctioneerId);
                await uow.Commit();
                return true;
            }
        }

        public async void AddReview(ReviewDto review)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                reviewService.Create(review);
                review.Reviewed.Rating = await CalculateRating(review.ReviewedId);
                await auctioneerService.Update(review.Reviewed);
                await uow.Commit();
            }
        }

        public async Task<bool> DeleteReview(Guid ReviewId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var review = await reviewService.GetReviewEntity(ReviewId);
                if (review == null)
                {
                    return false;
                }
                var reviewed = await auctioneerService.GetAsync(review.ReviewedId);
                reviewService.DeleteProduct(ReviewId);
                reviewed.Rating = await CalculateRating(reviewed.Id);
                await auctioneerService.Update(reviewed);
                await uow.Commit();
                return true;
            }
        }

        private async Task<double> CalculateRating(Guid auctioneerId)
        {
            var reviews = await reviewService.GetAllReviewsForUser(auctioneerId);
            return reviews.Sum(x => x.Rating) / reviews.Count;
        }

        public async Task<IList<ProductDto>> GetAllActiveProductsUserPutBidsOn(Guid auctioneerId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var bids = await bidService.GetAllBidsForUser(auctioneerId);
                return  bids.Where(x => x.Product.ValidTo >= DateTime.Now).Select(z => z.Product).Distinct().ToList();
            }
        }

        public async Task AddMoneyToAuctioneer(string name, decimal amount)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var auctioneer = await GetAuctioneerAccordingToUsernameAsync(name);
                auctioneer.Money += amount;
                await auctioneerService.Update(auctioneer);
                await uow.Commit();
            }
        }

        public async Task UpdateInfoToAuctioneer(string name, string info)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var auctioneer = await GetAuctioneerAccordingToUsernameAsync(name);
                auctioneer.Info = info;
                await auctioneerService.Update(auctioneer);
                await uow.Commit();
            }
        }
    }
}
