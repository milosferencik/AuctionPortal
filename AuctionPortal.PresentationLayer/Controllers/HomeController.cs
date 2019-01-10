using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.Facades;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {
        public ProductFacade ProductFacade { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {
            CategoryDto categoryDto = new CategoryDto();
            categoryDto.Name = "mobily";
            categoryDto.Id = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f");
            await ProductFacade.CreateCategoryAsync(categoryDto);
            return RedirectToAction("Index", "Home");
        }
    }
}