﻿using AuctionPortal.BusinessLayer.DataTransferObjects;
using AuctionPortal.BusinessLayer.DataTransferObjects.Filters;
using PagedList;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AuctionPortal.PresentationLayer.Models
{
    public class ProductListViewModel
    {
        public string[] ProductSortCriteria => new[] { nameof(ProductDto.Name) };

        public IList<CategoryDto> Categories { get; set; }

        public IPagedList<ProductDto> Products { get; set; } 

        public ProductFilterDto Filter { get; set; }

        public SelectList AllSortCriteria => new SelectList(ProductSortCriteria);
    }
}