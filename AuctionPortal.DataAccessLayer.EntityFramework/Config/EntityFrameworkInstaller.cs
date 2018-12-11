using AuctionPortal.Infrastructure.UnitOfWork;
using AuctionPortal.Infrastructure.EntityFramework;
using AuctionPortal.Infrastructure.EntityFramework.UnitOfWork;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Data.Entity;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.Query;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Config
{
    public class EntityFrameworkInstaller : IWindsorInstaller
    {
        internal const string ConnectionString = "Data source=(localdb)\\mssqllocaldb;Database=AuctionPortalDatabaseSample;Trusted_Connection=True;MultipleActiveResultSets=true";
        //internal const string ConnectionString = "Server=tcp:pv179-ferencik.database.windows.net,1433;Initial Catalog=newestAuctionPortal;Persist Security Info=False;User ID=milosferencik;Password=Kto-to-vie;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<DbContext>>()
                    .Instance(() => new AuctionPortalDbContext())
                    .LifestyleTransient(),
                Component.For<IUnitOfWorkProvider>()
                    .ImplementedBy<EntityFrameworkUnitOfWorkProvider>()
                    .LifestyleSingleton(),
                Component.For(typeof(IRepository<>))
                    .ImplementedBy(typeof(EntityFrameworkRepository<>))
                    .LifestyleTransient(),
                Component.For(typeof(IQuery<>))
                    .ImplementedBy(typeof(EntityFrameworkQuery<>))
                    .LifestyleTransient()
            );
        }
    }
}
