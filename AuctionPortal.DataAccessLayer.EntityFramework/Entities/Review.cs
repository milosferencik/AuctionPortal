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

        /// <summary>
        /// Person that creates review.
        /// </summary>
        public virtual Auctioneer Reviewer { get; set; }

        [ForeignKey(nameof(Reviewed))]
        public Guid ReviewedId { get; set; }

        /// <summary>
        /// Person that is reviewed.
        /// </summary>
        public virtual Auctioneer Reviewed { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [MaxLength(65536)]
        public string Info { get; set; }
    }
}
