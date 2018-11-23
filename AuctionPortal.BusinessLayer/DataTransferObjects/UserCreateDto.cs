using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class UserCreateDto : DtoBase
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 30")]
        public string Password { get; set; }

        [Required(ErrorMessage = "First name is required!")]
        [MaxLength(64, ErrorMessage = "Your first name is too long!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "This is not valid email address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        [MaxLength(1024)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Birthdate is required!")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MaxLength(128)]
        public string Info { get; set; }

    }
}
