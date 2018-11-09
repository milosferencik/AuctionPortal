using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class BidDto : DtoBase
    {
        public Guid ProductId { get; set; }

        public ProductDto Product { get; set; }

        public Guid BidderId { get; set; }

        public AuctioneerDto Bidder { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public DateTime TimeOfBid { get; set; }
    }
}
