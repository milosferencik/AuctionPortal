using AuctionPortal.BusinessLayer.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuctionPortal.PresentationLayer.Models
{
    public class CreateProductModel
    {
        public IList<CategoryDto> Categories { get; set; }

        public ProductDto Product { get; set; }
    }
}