using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}