using AuctionPortal.DataAccessLayer.EntityFramework.Config;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AuctionPortal.DataAccessLayer.EntityFramework
{
    public class AuctionPortalDbContext : DbContext
    {
        //private const string ConnectionString = "Server=tcp:pv179-ferencik.database.windows.net,1433;Initial Catalog=MyDatabase;Persist Security Info=False;User ID=milosferencik;Password=Kto-to-vie;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public AuctionPortalDbContext() : base(EntityFrameworkInstaller.ConnectionString)
        {
            //Database.SetInitializer<AuctionPortalDbContext>(new AuctionPortalDBInitializer());
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public AuctionPortalDbContext(DbConnection connection) : base(connection, true)
        {
            Database.CreateIfNotExists();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Auctioneer> Auctioneers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
