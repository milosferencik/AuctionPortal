using System;
using System.Threading.Tasks;
using AuctionPortal.DataAccessLayer.EntityFramework.Entities;
using AuctionPortal.Infrastructure;
using AuctionPortal.Infrastructure.UnitOfWork;
using NUnit.Framework;

namespace AuctionPortal.DataAccessLayer.EntityFramework.Tests.RepositoryTests
{
    public class ProductRepositoryTests
    {
        private readonly IUnitOfWorkProvider unitOfWorkProvider = Initializer.Container.Resolve<IUnitOfWorkProvider>();

        private readonly IRepository<Product> productRepository = Initializer.Container.Resolve<IRepository<Product>>();

        private readonly Guid androidCategoryId = Guid.Parse("aa02dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid samsungGalaxyJ7Id = Guid.Parse("aa05dc64-5c07-40fe-a916-175165b9b90f");

        private readonly Guid xiaomiMi5Id = Guid.Parse("aa12dc64-5c07-40fe-a916-175165b9b90f");

        [Test]
        public async Task GetProductAsync_AlreadyStoredInDBNoIncludes_ReturnsCorrectProduct()
        {
            // Arrange
            Product samsungGalaxyJ7Product;

            // Act
            using (unitOfWorkProvider.Create())
            {
                samsungGalaxyJ7Product = await productRepository.GetAsync(samsungGalaxyJ7Id);
            }

            // Assert
            Assert.AreEqual(samsungGalaxyJ7Product.Id, samsungGalaxyJ7Id);
        }

        [Test]
        public async Task GetProductAsync_AlreadyStoredInDBWithIncludes_ReturnsCorrectProductWithInitializedParent()
        {
            Product samsungGalaxyJ7Product;

            using (unitOfWorkProvider.Create())
            {
                samsungGalaxyJ7Product = await productRepository.GetAsync(samsungGalaxyJ7Id, nameof(Product.Category));
            }

            Assert.IsTrue(samsungGalaxyJ7Product.Id.Equals(samsungGalaxyJ7Id) && samsungGalaxyJ7Product.Category.Id.Equals(androidCategoryId));
        }

        [Test]
        public async Task CreateProductAsync_ProductIsNotPreviouslySeeded_CreatesNewProduct()
        {
            Product xiaomiMi5 = new Product
            {
                CategoryId = androidCategoryId,
                Info = "Xiaomi Mi 5 smartphone was launched in February 2016. The phone comes with a 5.15-inch touchscreen display with a resolution of 1080 pixels by 1920 pixels at a PPI of 428 pixels per inch. The Xiaomi Mi 5 is powered by 1.3GHz quad - core Qualcomm Snapdragon 820 processor and it comes with 3GB of RAM.The phone packs 32GB of internal storage cannot be expanded.As far as the cameras are concerned, the Xiaomi Mi 5 packs a 16-megapixel primary camera on the rear and a 4-megapixel front shooter for selfies. The Xiaomi Mi 5 runs Android 6.0 and is powered by a 3000mAh non removable battery. It measures 144.50 x 69.20 x 7.25 (height x width x thickness) and weighs 129.00 grams. The Xiaomi Mi 5 is a dual SIM(GSM and GSM) smartphone that accepts two Nano-SIM.Connectivity options include Wi-Fi, GPS, Bluetooth, NFC, 3G, 4G(with support for Band 40 used by some LTE networks in India). Sensors on the phone include Proximity sensor, Ambient light sensor, Accelerometer, and Gyroscope.",
                Name = "Xiaomi Mi 5",
                ProductImgUri = @"\Content\Images\Products\xiaomi_mi_5.jpeg",
                MinimalBid = 10,
                ValidTo = DateTime.Today,
                SellerId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b989"),
                BuyerId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b989")
            };

            using (var uow = unitOfWorkProvider.Create())
            {
                
                productRepository.Create(xiaomiMi5);
                await uow.Commit();
            }
            Assert.IsTrue(!xiaomiMi5.Id.Equals(Guid.Empty));
        }


        [Test]
        public async Task UpdateProductAsync_ProductIsPreviouslySeeded_UpdatesProduct()
        {
            Product updatedAndroidProduct;

            var newSamsungGalaxyJ7 = new Product
            {
                Id = samsungGalaxyJ7Id,
                CategoryId = androidCategoryId,
                Info = "Designed with all the features you love, the Samsung Galaxy J7 is the smartphone you’ve been waiting for. Watching a movie or reading a book is more enjoyable on the large 5.5 HD Super AMOLED display.And while the 13MP main camera captures clearer photos, the 5MP front camera gives you more flattering selfies in any light. Best of all, the long-lasting battery means the Samsung Galaxy J7 keeps up with you.",
                Name = "Samsung Galaxy J7 2015",
                ProductImgUri = @"\Content\Images\Products\samsung_galaxy_J7.jpeg",
                MinimalBid = 15,
                ValidTo = new DateTime(2018, 11, 11),
                SellerId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b989"),
                BuyerId = Guid.Parse("aa01dc64-5c07-40fe-a916-175165b9b989")
            };

            using (var uow = unitOfWorkProvider.Create())
            {
                productRepository.Update(newSamsungGalaxyJ7);
                await uow.Commit();
                updatedAndroidProduct = await productRepository.GetAsync(samsungGalaxyJ7Id);
            }

            Assert.IsTrue(newSamsungGalaxyJ7.Name.Equals(updatedAndroidProduct.Name) && newSamsungGalaxyJ7.ValidTo.Equals(updatedAndroidProduct.ValidTo) && newSamsungGalaxyJ7.MinimalBid.Equals(updatedAndroidProduct.MinimalBid));
        }

        [Test]
        public async Task DeleteProductAsync_ProductIsPreviouslySeeded_DeletesProduct()
        {
            Product deletedAndroidProduct;

            using (var uow = unitOfWorkProvider.Create())
            {
                productRepository.Delete(samsungGalaxyJ7Id);
                await uow.Commit();
                deletedAndroidProduct = await productRepository.GetAsync(samsungGalaxyJ7Id);
            }

            Assert.AreEqual(deletedAndroidProduct, null);
        }
    }
}