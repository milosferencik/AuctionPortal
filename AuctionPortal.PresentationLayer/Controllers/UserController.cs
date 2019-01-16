using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.PresentationLayer.Helpers;
using AuctionPortal.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        public AuctioneerFacade AuctioneerFacade { get; set; }
        public const int PageSize = 9;

        private const string FilterSessionKey = "filter";

        [HttpPost]
        public async Task<ActionResult> Index(AuctioneerListViewModel model)
        {
            model.Filter.PageSize = PageSize;
            Session[FilterSessionKey] = model.Filter;

            var result = await AuctioneerFacade.GetAuctioneersAsync(model.Filter);
            var newModel = await InitializeAuctioneerListViewModel(result);
            return View("UserListView", newModel);
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var filter = Session[FilterSessionKey] as AuctioneerFilterDto ?? new AuctioneerFilterDto { PageSize = PageSize };
            filter.RequestedPageNumber = page;
            var result = await AuctioneerFacade.GetAuctioneersAsync(filter);

            var model = await InitializeAuctioneerListViewModel(result);
            return View("UserListView", model);
        }

        private async Task<AuctioneerListViewModel> InitializeAuctioneerListViewModel(QueryResultDto<AuctioneerDto, AuctioneerFilterDto> result)
        {
            return new AuctioneerListViewModel
            {
                Auctioneers = new StaticPagedList<AuctioneerDto>(result.Items, result.RequestedPageNumber ?? 1, PageSize, (int)result.TotalItemsCount),
                Filter = result.Filter
            };
        }

        public async Task<ActionResult> ShowMyProfile()
        {
            var name = User.Identity.Name;
            var user = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(name);
            return View("UserInfo",user);
        }

        [HttpGet]
        [Route("DeleteAuctioneer/{id}")]
        public async Task<ActionResult> DeleteAuctioneer(Guid id)
        {
            bool successful = await AuctioneerFacade.DeleteAuctioneer(id);
            if (!successful)
            {
                return View("Error", new ErrorModel { Message = "You can't delete user which is still selling products." });
            }
            return View("OperationSuccesful");
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "addinfo")]
        public async Task<ActionResult> AddInfo(AuctioneerDto auctioneer)
        {
            await AuctioneerFacade.UpdateInfoToAuctioneer(User.Identity.Name, auctioneer.Info);
            return RedirectToAction("ShowMyProfile");
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "addmoney")]
        public async Task<ActionResult> AddMoney(AuctioneerDto auctioneer)
        {
            await AuctioneerFacade.AddMoneyToAuctioneer(User.Identity.Name, auctioneer.Money);
            return RedirectToAction("ShowMyProfile");
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "createReview")]
        public async Task<ActionResult> CreateReview(AuctioneerDto auctioneer)
        {
            var me = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(User.Identity.Name);
            var review = new ReviewDto {
                Info = auctioneer.NevReview.Info,
                Rating = auctioneer.NevReview.Rating,
                ReviewedId = auctioneer.Id,
                ReviewerId = me.Id
            };
            await AuctioneerFacade.AddReview(review);
            return RedirectToAction("Details", new { id = auctioneer.Id});
        }

        [HttpGet]
        [Route("Details/{id}")]
        public async Task<ActionResult> Details(Guid id)
        {
            var model = await AuctioneerFacade.GetAuctioneerAsync(id);
            var reviews = await AuctioneerFacade.GetAllReviewsForUser(id);
            if (reviews != null)
            {
                model.Review = reviews;
            }
            return View("UserInfo", model);
        }

        public ActionResult NotLoggedIn()
        {
            return View("NotLogged");
        }
    }
}