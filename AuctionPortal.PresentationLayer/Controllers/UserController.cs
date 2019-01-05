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
            var user = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(User.Identity.Name);
            return View("UserInfo", user);
        }
    }
}