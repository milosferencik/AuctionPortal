using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using AuctionPortal.BusinessLayer.Facades;
using AuctionPortal.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Controllers
{
    public class ProductsController : Controller
    {
        public const int PageSize = 9;

        private const string FilterSessionKey = "filter";
        private const string CategoryTreesSessionKey = "categoryTrees";

        public ProductFacade ProductFacade { get; set; }


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
                //Products = new StaticPagedList<ProductDto>(result.Items, result.RequestedPageNumber ?? 1, PageSize, (int)result.TotalItemsCount),
                Products = new List<ProductDto>(result.Items),
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