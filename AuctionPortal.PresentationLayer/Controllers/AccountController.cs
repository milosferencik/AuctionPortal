using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.PresentationLayer.Models;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        public AuctioneerFacade AuctioneerFacade { get; set; }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserCreateDto userCreateDto)
        {
            try
            {
                await AuctioneerFacade.RegisterUser(userCreateDto);

                var authTicket = new FormsAuthenticationTicket(1, userCreateDto.Username, DateTime.Now,
                    DateTime.Now.AddMinutes(30), false, "");
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("Username", "Account with that username already exists!");
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            (bool success, bool isAdmin) = await AuctioneerFacade.Login(model.Username, model.Password);
            if (success)
            {
                var authTicket = new FormsAuthenticationTicket(1, model.Username, DateTime.Now,
                    DateTime.Now.AddMinutes(30),false, isAdmin.ToString());
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                var decodedUrl = "";
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    decodedUrl = Server.UrlDecode(returnUrl);
                }

                if (Url.IsLocalUrl(decodedUrl))
                {
                    return Redirect(decodedUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Wrong username or password!");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> BecomeAdmin()
        {
            var name = User.Identity.Name;
            var me = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(name);
            me.IsAdmin = true;
            await AuctioneerFacade.UpdateAuctioneer(me);
            return RedirectToAction("Index", "Home");
        }
        
    }
}