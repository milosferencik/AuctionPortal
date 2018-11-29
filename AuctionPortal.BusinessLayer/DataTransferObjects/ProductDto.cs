using AuctionPortal.BusinessLayer.DataTransferObjects.Common;
using System;
using System.Collections.Generic;

namespace AuctionPortal.BusinessLayer.DataTransferObjects
{
    public class ProductDto : DtoBase
    {
        public string Name { get; set; }

        public decimal MinimalBid { get; set; }

        public decimal StartPrice { get; set; }

        public string Info { get; set; }

        public Guid SellerId { get; set; }

        public AuctioneerDto Seller { get; set; }

        public DateTime ValidTo { get; set; }

        public string ProductImgUri { get; set; }

        public Guid CategoryId { get; set; }

        public CategoryDto Category { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return obj.GetType() == this.GetType() && ((ProductDto)obj).GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (!Id.Equals(Guid.Empty))
            {
                return Id.GetHashCode();
            }
            unchecked
            {
                var hashCode = Name?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Info?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ MinimalBid.GetHashCode();
                hashCode = (hashCode * 397) ^ ValidTo.GetHashCode();
                hashCode = (hashCode * 397) ^ SellerId.GetHashCode();
                hashCode = (hashCode * 397) ^ (ProductImgUri?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ CategoryId.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
