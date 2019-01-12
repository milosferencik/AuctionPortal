using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System.ComponentModel.DataAnnotations;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class UserDto : DtoBase
    {
        [Required]
        public string Username { get; set; }

        [Required, StringLength(100)]
        public string PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string PasswordHash { get; set; }

        public bool IsAdmin { get; set; }
    }
}
