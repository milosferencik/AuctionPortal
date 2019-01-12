using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class AuctioneerDto : DtoBase
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public DateTime BirthDate { get; set; }

        public decimal Money { get; set; }

        public string Info { get; set; }

        public double Rating { get; set; }

        public bool IsAdmin { get; set; }
    }
}
