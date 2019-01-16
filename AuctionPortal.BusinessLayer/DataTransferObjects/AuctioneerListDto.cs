using System.Collections.Generic;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    class AuctioneerListDto
    {
        public IEnumerable<AuctioneerDto> Products { get; set; } = new List<AuctioneerDto>();
    }
}
