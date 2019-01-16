using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System.ComponentModel.DataAnnotations;

namespace AuctionPortal.BusinessLayer.DataTransferObjects.Filters
{
    public class AuctioneerFilterDto : FilterDtoBase
    {
        [EmailAddress(ErrorMessage = "This is not valid email address!")]
        public string Email { get; set; }
    }
}
