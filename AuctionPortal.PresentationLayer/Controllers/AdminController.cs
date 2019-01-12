using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.PresentationLayer.Helpers;
using AuctionPortal.PresentationLayer.Models;
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

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "deleteUser")]
        public async Task<ActionResult> DeleteUser(AdminModel model)
        {
            try
            {
                var auctioneer = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(model.Name);
                bool successful = await AuctioneerFacade.Delete(auctioneer.Id);
                if (!successful)
                {
                    return View("Error", new ErrorModel { Message = "You can't delete user which is still selling products." });
                }
                return View("OperationSuccesful");

            } catch (NullReferenceException)
            {
                return View("Error", new ErrorModel { Message = "User doesn't exist." });
            }
        }
    }
}