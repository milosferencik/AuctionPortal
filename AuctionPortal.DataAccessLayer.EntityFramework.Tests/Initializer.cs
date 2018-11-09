using System.Data.Entity;
using Castle.Windsor;
using AuctionPortal.DataAccessLayer.EntityFramework.Tests.Config;
using NUnit.Framework;
using AuctionPortal.DataAccessLayer.EntityFramework;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Tests
{
    [SetUpFixture]
    public class Initializer
    {
        internal static readonly IWindsorContainer Container = new WindsorContainer();

        /// <summary>
        /// Initializes all Business Layer tests
        /// </summary>
        [OneTimeSetUp]
        public void InitializeBusinessLayerTests()
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();
            Database.SetInitializer(new DropCreateDatabaseAlways<AuctionPortalDbContext>());
            Container.Install(new EntityFrameworkTestInstaller());
        }
    }
}
