﻿using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;

namespace AuctionPortal.BusinessLayer.DataTransferObjects.Filters
{
    public class ProductFilterDto : FilterDtoBase
    {
        public Guid[] CategoryIds { get; set; }

        public string[] CategoryNames { get; set; }

        public string SearchedName { get; set; }

        public Guid SellerId { get; set; }

        public Guid BuyerID { get; set; }

        public bool? IsSold { get; set; }
    }
}
