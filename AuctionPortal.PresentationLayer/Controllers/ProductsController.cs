using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.Facades;
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

        private const string OtherCategory = "ab01dc64-5c07-40fe-a916-175165b9b90f";
        private const string FilterSessionKey = "filter";
        private const string CategoryTreesSessionKey = "categoryTrees";

        public ProductFacade ProductFacade { get; set; }
        public AuctioneerFacade AuctioneerFacade { get; set; }

        [HttpPost]
        public async Task<ActionResult> Index(ProductListViewModel model)
        {
            model.Filter.PageSize = PageSize;
            model.Filter.CategoryIds = ProcessCategoryIds(model.Categories);
            if (model.Offer != null)
            {
                model.Filter.IsSold = (model.Offer.Equals("Sold")) ? true : false;
            }
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

        public async Task<ActionResult> SoldProducts(Guid id)
        {
            var filter = new ProductFilterDto { SellerId = id, IsSold = true, PageSize = 9 } ;
            var result = await ProductFacade.GetProductsAsync(filter);
            var newModel = await InitializeProductListViewModel(result);
            newModel.Status = "Sold Products";
            return View("ProductListView", newModel);
        }

        public async Task<ActionResult> ActualProducts(Guid id)
        {
            var model = new ProductListViewModel { Filter = new ProductFilterDto { SellerId = id, IsSold = false, PageSize = 9 } };
            var result = await ProductFacade.GetProductsAsync(model.Filter);
            var newModel = await InitializeProductListViewModel(result);
            newModel.Status = "Actual Offered Products";
            return View("ProductListView", newModel);
        }

        public async Task<ActionResult> BoughtProducts(Guid id)
        {
            var model = new ProductListViewModel { Filter = new ProductFilterDto { BuyerID = id, IsSold = true, PageSize = 9 } };
            var result = await ProductFacade.GetProductsAsync(model.Filter);
            var newModel = await InitializeProductListViewModel(result);
            newModel.Status = "Bought Products";
            return View("ProductListView", newModel);
        }

        public ActionResult ClearFilter()
        {
            Session[FilterSessionKey] = null;
            Session[CategoryTreesSessionKey] = null;
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(Guid id)
        {
            var model = await ProductFacade.EvaluateProduct(id);
            model.Category = await ProductFacade.GetCategoryAsync(model.CategoryId);
            return View("ProductDetailView", model);
        }

        public async Task<ActionResult> CreateNewProduct()
        {
            var categories = await ProductFacade.GetAllCategories(); 
            var model = new CreateProductModel { Categories = categories.ToList()};
            return View("ProductCreateView", model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewProduct(CreateProductModel model)
        {
            model.Product.CategoryId = ProcessCategoryIds(model.Categories).LastOrDefault();
            if (model.Product.CategoryId.Equals(Guid.Empty) )
            {
                model.Product.CategoryId = Guid.Parse(OtherCategory);
            }
            var seller = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(User.Identity.Name);
            model.Product.SellerId = seller.Id;
            var prodID = await ProductFacade.CreateProductAsync(model.Product);
            return RedirectToAction("Details", new { id = prodID });
        }

        [HttpGet]
        [Route("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await ProductFacade.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultiPostAction(Name = "action", Argument = "createBid")]
        public async Task<ActionResult> CreateBid(ProductDto productDto)
        {
            var me = await AuctioneerFacade.GetAuctioneerAccordingToUsernameAsync(User.Identity.Name);
            var product = await ProductFacade.GetProductAsync(productDto.Id);
            var bidDto = new BidDto
            {
                ProductId = product.Id,
                Product = product,
                Price = productDto.Bid.Price,
                TimeOfBid = DateTime.Now,
                BidderId = me.Id,
                Bidder = me
            };
            
            try
            {
               await ProductFacade.CreateBidAsync(bidDto);
            }catch(Exception x )
            {
                return View("Error", new ErrorModel { Message = x.Message });
            }
            return RedirectToAction("Details", new { id = productDto.Id });
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
        private static Guid[] ProcessCategoryIds(IList<CategoryDto> model)
        {
            var selectedCategoryIds = new List<Guid>();
            foreach (var categoryTreeRoot in model)
            {
                if (categoryTreeRoot.IsActive)
                {
                    selectedCategoryIds.Add(categoryTreeRoot.Id);
                    selectedCategoryIds.AddRange(model
                        .Where(node => node.ParentId == categoryTreeRoot.Id && node.IsActive)
                        .Select(node => node.Id));
                }
            }
            return selectedCategoryIds.ToArray();
        }

        #endregion

    }
}