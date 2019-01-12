using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.PresentationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        public AuctioneerFacade AuctioneerFacade { get; set; }

        public async Task<ActionResult> ShowMyProfile()
        {
            var name = User.Identity.Name;
            var user = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(name);
            return View("UserInfo",user);
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

        public async Task<ActionResult> Details(Guid id)
        {
            if(id == null)
            {
                NotLoggedIn();
            }
            var model = await AuctioneerFacade.GetAuctioneerAsync(id);
            return View("UserInfo", model);
        }

        public ActionResult NotLoggedIn()
        {
            return View("NotLogged");
        }
    }
}