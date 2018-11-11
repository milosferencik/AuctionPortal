using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class ReviewDto : DtoBase
    {
        public Guid ReviewerId { get; set; }

        public AuctioneerDto Reviewer { get; set; }

        public Guid ReviewedId { get; set; }

        public AuctioneerDto Reviewed { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }

        [MaxLength(65536)]
        public string Info { get; set; }
    }
}
