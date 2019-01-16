using AuctionPortal.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Entities
{
    public class Product : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(AuctionPortalDbContext.Products);

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public decimal MinimalBid { get; set; }

        [Range(0, int.MaxValue)]
        public decimal StartPrice { get; set; }

        [MaxLength(65536)]
        public string Info { get; set; }

        [ForeignKey(nameof(Seller))]
        public Guid SellerId { get; set; }

        public virtual Auctioneer Seller { get; set; }

        //[ForeignKey(nameof(Buyer))]
        public Guid BuyerId { get; set; }

        //public virtual Auctioneer Buyer { get; set; }
        
        [Required]
        public DateTime ValidTo { get; set; }

        public DateTime SoldTime { get; set; } = DateTime.Parse("1/1/1800");

        public bool IsSold { get; set; }

        [MaxLength(1024)]
        public string ProductImgUri { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

    }
}
