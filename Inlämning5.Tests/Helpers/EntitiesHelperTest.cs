using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes;
using Inlämning5.Classes.Helpers;
using Xunit;

namespace Inlämning5.Tests.Helpers
{
    public class EntitiesHelperTest
    {
        IEnumerable<Product> CreateTestProducts()
        {
            List<Product> products = new List<Product>() {
                new Product(){
                    Id = "1",
                    Name = "Mixer",
                    Manufacturer= new Manufacturer(){ Name = "Braun"},
                    Shops = new List<Shop>() { new Shop("Stockholm"), new Shop("Orust"), new Shop("Malmö") },
                    Price = 390
                },
                new Product(){
                    Id = "2",
                    Name = "Radio",
                    Manufacturer= new Manufacturer(){ Name = "Philips"},
                    Shops = new List<Shop>() { new Shop("Göteborg"), new Shop("Stockholm") },
                    Price = 250
                }
            };
            return products;
        }
        List<Shop> CreateTestShops()
        {
            List<Shop> shops = new List<Shop>() {
                new Shop("Stockholm"){ Id = "1"},
                new Shop("Malmö"){ Id = "2"},
                new Shop("Göteborg"){ Id = "3"},
                new Shop("Orust"){ Id = "4"}
            };
            return shops;
        }
        EntitiesHelper CreateEntitiesHelper()
        {
            var productsRepo = new FakeProductsRepository(CreateTestProducts());
            var shopsRepo = new FakeShopsRepository(CreateTestShops());
            return new EntitiesHelper(productsRepo, shopsRepo);
        }

        [Fact]
        public void UpdateShopsWithinProductShouldReturnAProductWithAPopulatedListOfShops()
        {
            var _cut = CreateEntitiesHelper();
            Product product = new Product(){
                Id = "123",
                Name = "Stavmixer",
                Manufacturer = new Manufacturer() { Name = "Braun" },
                Price = 390

            };
            string shopName = "Stockholm";
            
            product = _cut.UpdateShopsWithinProduct(product, shopName);
                        
            Assert.Equal("Stockholm", product.Shops.First().Name);

        }

        [Fact]
        public void GetProductByNameShouldReturnProductDetailsWhenMatchesAreFound()
        {
            var _cut = CreateEntitiesHelper();
            string productName = "Radio";
            
            var actualProduct = _cut.GetProductByName(productName);

            Assert.Equal("2", actualProduct.First().Id);
            Assert.Equal("Radio", actualProduct.First().Name);
            Assert.Equal(250, actualProduct.First().Price);
        }
        [Fact]
        public void GetProductByNameShouldReturnAnEmptyListWhenNoMatchesAreFound()
        {
            var _cut = CreateEntitiesHelper();
            var productName = "Kokvåg";
            
            var actualProduct = _cut.GetProductByName(productName);

            Assert.False(actualProduct.Any());
        }
        [Fact]
        public void GetShopByNameShouldReturnShopDetailsWhenMatchesAreFound()
        {
            var _cut = CreateEntitiesHelper();
            var shopName = "Malmö";
            
            var actualShop = _cut.GetShopByName(shopName);

            Assert.Equal("2", actualShop.First().Id);
        }
        [Fact]
        public void GetShopByNameShouldReturnAnEmptyListWhenNoMatchesAreFound()
        {
            var _cut = CreateEntitiesHelper();
            var shopName = "Uddevalla";
            
            var actualShop = _cut.GetShopByName(shopName);

            Assert.False(actualShop.Any());
        }
        [Fact]
        public void UpdateShopCollectionShouldNotUpdateShopsListWhenShopAlreadyRegistered()
        {
            var _cut = CreateEntitiesHelper();
            var shopName = "Uddevalla";
            
            var actualAddedShop = _cut.AddShopToCollection(shopName);

            Assert.NotNull(actualAddedShop);
        }
        [Fact]
        public void UpdatePriceExistingProductShouldThrowAnInvalidOperationExceptionWhenProductIsNotFound()
        {
            var _cut = CreateEntitiesHelper();
            var productToUpdate = new Product()
            {
                Name = "Kaffekvarn",
                Price = 390
            };
            
            Assert.Throws<InvalidOperationException>(()=>_cut.UpdateProductPrice(productToUpdate));
        }
    }
}
