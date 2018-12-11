using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Models
{
    public class ProductListViewModel
    {
        public string[] ProductSortCriteria => new[] { nameof(ProductDto.Name) };

        public IList<CategoryDto> Categories { get; set; }

        public IList<ProductDto> Products { get; set; } //IPagedList

        public ProductFilterDto Filter { get; set; }

        public SelectList AllSortCriteria => new SelectList(ProductSortCriteria);
    }
}