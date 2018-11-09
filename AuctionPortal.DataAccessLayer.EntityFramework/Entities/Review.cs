using AuctionPortal.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Entities
{
    public class Review : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        public string TableName { get; } = nameof(AuctionPortalDbContext.Reviews);

        [ForeignKey(nameof(Reviewer))]
        public Guid ReviewerId { get; set; }

        public virtual Auctioneer Reviewer { get; set; }

        [ForeignKey(nameof(Reviewee))]
        public Guid? RevieweeId { get; set; }

        public virtual Auctioneer Reviewee { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [MaxLength(65536)]
        public string Info { get; set; }
    }
}
