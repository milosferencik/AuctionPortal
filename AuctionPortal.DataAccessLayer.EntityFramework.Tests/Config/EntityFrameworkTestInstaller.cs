using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.EntityFramework;
using AuctionPortal.Infrastructure.EntityFramework.UnitOfWork;
using AuctionPortal.Infrastructure.Query;
using AuctionPortal.Infrastructure.UnitOfWork;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;


namespace AuctionPortal.DataAccessLayer.EntityFramework.Tests.Config
{
    public class EntityFrameworkTestInstaller : IWindsorInstaller
    {
        private const string TestDbConnectionString = "InMemoryTestDBDemoEshop";

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<Func<DbContext>>()
                    .Instance(InitializeDatabase)
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

        private static DbContext InitializeDatabase()
        {
            var context = new AuctionPortalDbContext(Effort.DbConnectionFactory.CreatePersistent(TestDbConnectionString));
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            context.Auctioneers.RemoveRange(context.Auctioneers);
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();

            var smartphones = new Category { Id = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b90f"), Name = "Smartphones", Parent = null, ParentId = null };

            var android = new Category { Id = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f"), Name = "Android", Parent = smartphones, ParentId = smartphones.Id };

            var iOS = new Category { Id = Guid.Parse("aa04dc64-5c07-40fe-a916-175165b9b90f"), Name = "iOS", Parent = smartphones, ParentId = smartphones.Id };

            context.Categories.AddOrUpdate(category => category.Id, smartphones, android, iOS);
            var ja = new Auctioneer { Id = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b988"), Username = "milos", IsAdmin = "no", PasswordSalt = "ano", PasswordHash = "ano", Money = Decimal.One };
            context.Auctioneers.Add(ja);
            var samsungGalaxyJ7 = new Product
            {
                Id = Guid.Parse("aa05dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Info = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                Name = "Samsung Galaxy J7",
                MinimalBid = 10,
                ValidTo = DateTime.Today,
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg",
                SellerId = ja.Id,
                BuyerId = ja.Id
            };
            var lgG5 = new Product
            {
                Id = Guid.Parse("aa06dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Info = "LG G5 comes with a 5.30-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 554 pixels per inch. The LG G5 is powered by 2.15GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 200GB via a microSD card.As far as the cameras are concerned, the LG G5 packs a 16-megapixel primary camera on the rear and a 8-megapixel front shooter for selfies. The LG G5 runs Android 6.0.1 and is powered by a 2800mAh removable battery.It measures 149.40 x 73.90 x 7.70 (height x width x thickness) and weighs 159.00 grams. The LG G5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, FM, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                Name = "LG G5",
                MinimalBid = 12,
                ValidTo = new DateTime(2018, 11, 11),
                ProductImgUri = @"\Content\Images\Products\LG-G5.jpg",
                SellerId = ja.Id,
                BuyerId = ja.Id
            };
            var htc10 = new Product
            {
                Id = Guid.Parse("aa09dc64-5c07-40fe-a916-175165b9b90f"),
                Category = android,
                CategoryId = android.Id,
                Info = "HTC 10 smartphone was launched in April 2016. The phone comes with a 5.20-inch touchscreen display with a resolution of 1440 pixels by 2560 pixels at a PPI of 564 pixels per inch. The HTC 10 is powered by 1.6GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 4GB of RAM.The phone packs 32GB of internal storage that can be expanded up to 2000GB via a microSD card. As far as the cameras are concerned, the HTC 10 packs a 12-Ultrapixel primary camera on the rear and a 5-megapixel front shooter for selfies.The HTC 10 runs Android 6 and is powered by a 3000mAh non removable battery.It measures 145.90 x 71.90 x 9.00 (height x width x thickness) and weighs 161.00 grams. The HTC 10 is a single SIM(GSM) smartphone that accepts a Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope. ",
                Name = "HTC 10",
                MinimalBid = 15,
                ValidTo = new DateTime(2018, 12, 1),
                ProductImgUri = @"\Content\Images\Products\HTC_10.jpg",
                SellerId = ja.Id,
                BuyerId = ja.Id
            };
            context.Products.AddOrUpdate(samsungGalaxyJ7);
            context.Products.AddOrUpdate(lgG5);
            context.Products.AddOrUpdate(htc10);

            context.SaveChanges();

            return context;
        }
    }
}
