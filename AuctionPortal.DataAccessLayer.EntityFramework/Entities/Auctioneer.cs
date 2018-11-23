using AuctionPortal.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Entities
{
    public class Auctioneer : User
    {
        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(1024)]
        public string Address { get; set; }

        public DateTime BirthDate { get; set; } = new DateTime(1950, 1, 1);

        [MaxLength(1024)]
        public string AuctioneerImgUri { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Money { get; set; }

        [MaxLength(65536)]
        public string Info { get; set; }

        [Range(0.0, 5.0)]
        public double Rating { get; set; }
        
    }
}
