using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System.ComponentModel.DataAnnotations;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class UserCreateDto : DtoBase
    {
        public string Username { get; set; }

        [StringLength(100)]
        public string PasswordSalt { get; set; }

        [StringLength(100)]
        public string PasswordHash { get; set; }

        public bool IsAdmin { get; set; }

        public virtual AuctioneerDto Auctioneer { get; set; }
    }
}
