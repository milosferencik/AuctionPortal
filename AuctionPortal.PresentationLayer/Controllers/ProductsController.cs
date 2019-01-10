using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.PresentationLayer.Models;
using X.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AuctionPortal.PresentationLayer.Helpers;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class ProductsController : Controller
    {
        public const int PageSize = 9;

        private const string FilterSessionKey = "filter";
        private const string CategoryTreesSessionKey = "categoryTrees";

        public ProductFacade ProductFacade { get; set; }
        public AuctioneerFacade AuctioneerFacade { get; set; }

        [HttpPost]
        public async Task<ActionResult> Index(ProductListViewModel model)
        {
            model.Filter.PageSize = PageSize;
            model.Filter.CategoryIds = ProcessCategoryIds(model);
            Session[FilterSessionKey] = model.Filter;
            Session[CategoryTreesSessionKey] = model.Categories;

            var result = await ProductFacade.GetProductsAsync(model.Filter);
            var newModel = await InitializeProductListViewModel(result, model.Categories);
            return View("ProductListView", newModel);
        }

        public async Task<ActionResult> Index(int page = 1)
        {
            var filter = Session[FilterSessionKey] as ProductFilterDto ?? new ProductFilterDto { PageSize = PageSize };
            filter.RequestedPageNumber = page;
            var result = await ProductFacade.GetProductsAsync(filter);

            var categoryTrees = Session[CategoryTreesSessionKey] as IList<CategoryDto>;
            var model = await InitializeProductListViewModel(result, categoryTrees);
            return View("ProductListView", model);
        }

        public ActionResult ClearFilter()
        {
            Session[FilterSessionKey] = null;
            Session[CategoryTreesSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var model = await ProductFacade.GetProductAsync(id);
            return View("ProductDetailView", model);
        }

        public ActionResult CreateNewProduct()
        {
            return View("ProductCreateView");
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewProduct(ProductDto productDto)
        {
            var seller = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(User.Identity.Name);
            productDto.SellerId = seller.Id;
            var prodID = await ProductFacade.CreateProductWithCategoryNameAsync(productDto, "mobily");
            var product = await ProductFacade.GetProductAsync(prodID);

            return View("ProductDetailView", product);
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "createBid")]
        public async Task<ActionResult> CreateBid(ProductDto productDto)
        {
            var bidDto = new BidDto();
            bidDto.ProductId = productDto.Id;
            bidDto.Price = productDto.ActualPrice.Value;
            bidDto.TimeOfBid = DateTime.Now;
            var me = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(User.Identity.Name);
            bidDto.BidderId = me.Id;
            //try
            //{
               await ProductFacade.CreateBidAsync(bidDto);
            //}catch(Exception x)
            // {
            //     Console.WriteLine(x.Message);
            // }
            var product = await ProductFacade.GetProductAsync(productDto.Id);
            return View("ProductDetailView", product);
        }

        #region Helper methods

        /// <summary>
        /// Initializes new ProductListViewModel according to its parameters
        /// </summary>
        /// <param name="result">Product list query result containing products page and related data</param>
        /// <param name="categories">List of category trees</param>
        /// <returns>Initialized instance of ProductListViewModel</returns>
        private async Task<ProductListViewModel> InitializeProductListViewModel(QueryResultDto<ProductDto, ProductFilterDto> result, IList<CategoryDto> categories = null)
        {
            return new ProductListViewModel
            {
                Products = new StaticPagedList<ProductDto>(result.Items, result.RequestedPageNumber ?? 1, PageSize, (int)result.TotalItemsCount),
                Categories = categories ?? await ProductFacade.GetAllCategories() as IList<CategoryDto>,
                Filter = result.Filter
            };
        }
    

        /// <summary>
        /// Processes category IDs by filtering out unchecked categories
        /// </summary>
        /// <param name="model">model containing category trees</param>
        /// <returns>selected categories</returns>
        private static Guid[] ProcessCategoryIds(ProductListViewModel model)
        {
            var selectedCategoryIds = new List<Guid>();
            foreach (var categoryTreeRoot in model.Categories)
            {
                if (categoryTreeRoot.IsActive)
                {
                    selectedCategoryIds.Add(categoryTreeRoot.Id);
                    selectedCategoryIds.AddRange(model.Categories
                        .Where(node => node.ParentId == categoryTreeRoot.Id && node.IsActive)
                        .Select(node => node.Id));
                }
            }
            return selectedCategoryIds.ToArray();
        }

        #endregion

    }
}