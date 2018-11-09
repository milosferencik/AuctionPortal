using AuctionPortal.BusinessLayer.DataTransferObjects.Common;

namespace AuctionPortal.BusinessLayer.DataTransferObjects.Filters
{
    public class AuctioneerFilterDto : FilterDtoBase
    {
        public string Email { get; set; }
    }
}
