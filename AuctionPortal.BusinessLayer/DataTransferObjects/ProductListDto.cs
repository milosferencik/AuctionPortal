using System.Collections.Generic;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class ProductListDto
    {
        public AuctioneerDto Auctioneer { get; set; }

        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();

    }
}
