using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;

namespace AuctionPortal.BusinessLayer.DataTransferObjects.Filters
{
    public class ReviewFilterDto : FilterDtoBase
    {
        /// <summary>
        /// Person that is reviewed.
        /// </summary>
        public Guid ReviewedId { get; set; }
    }
}
