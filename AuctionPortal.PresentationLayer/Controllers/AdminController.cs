using AuctionPortal.BusinessLayer.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class AdminController : Controller
    {
        public AuctioneerFacade AuctioneerFacade { get; set; }
        public ProductFacade ProductFacade { get; set; }

        public async Task<ActionResult> Show()
        {
            var name = User.Identity.Name;
            if (name == "" || name == null)
            {
                return View("NotLogged");
            }
            var me = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(name);
            if (!me.IsAdmin)
            {
                return View("NotAdmin");
            }
            return View("ShowOptions");
        }
    }
}