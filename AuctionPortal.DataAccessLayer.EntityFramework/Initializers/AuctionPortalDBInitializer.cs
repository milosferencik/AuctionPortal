using System.Data.Entity;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Initializers
{
    class AuctionPortalDBInitializer : CreateDatabaseIfNotExists<AuctionPortalDbContext>
    {
        protected override void Seed(AuctionPortalDbContext context)
        {
            base.Seed(context);
        }
    }
}
