using AuctionPortal.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AuctionPortal.DataAccessLayer.EntityFramework.Entities
{
    public class Bid : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(AuctionPortalDbContext.Bids);

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        [ForeignKey(nameof(Bidder))]
        public Guid BidderId { get; set; }

        public virtual User Bidder { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public DateTime TimeOfBid { get; set; }
    }
}
