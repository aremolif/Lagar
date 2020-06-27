using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes;
using Inlämning5.Tests;
using Xunit;

namespace Inlämning5.Tests
{
    public class ProduktFilterTest
    {
        IEnumerable<Produkt> CreateTestProducts()
        {
            List<Produkt> products = new List<Produkt>() {
                new Produkt(){
                    Id = "1",
                    Name = "Mixer",
                    Butik = new List<Butik>() { new Butik("Stockholm"), new Butik("Orust"), new Butik("Malmö") },
                    Price = 390
                },
                new Produkt(){
                    Id = "2",
                    Name = "Radio",
                    Butik = new List<Butik>() { new Butik("Göteborg"), new Butik("Stockholm") },
                    Price = 250
                }
            };
            return products;
        }
        IEnumerable<Butik> CreateTestShops()
        {
            List<Butik> shops = new List<Butik>() {
                new Butik("Stockholm"){ Id = "1"},
                new Butik("Malmö"){ Id = "2"},
                new Butik("Göteborg"){ Id = "3"},
                new Butik("Orust"){ Id = "4"}
            };
            return shops;
        }
        ProduktFilter CreateProduktFilter()
        {
            var productsRepo = new FakeProductsRepository(CreateTestProducts());
            var shopsRepo = new FakeShopsRepository(CreateTestShops());
            return new ProduktFilter(productsRepo, shopsRepo);
        }
        
        [Fact]
        public void SearchByPriceShouldReturnAnEmptyListOfPoductsFilteredByPriceWhenNoMatchesAreFound()
        {
            
            var _cut = CreateProduktFilter();

            var maxPrice = 100;

            var actualListOfFilteredProduct = _cut.SearchByPrice(maxPrice);
            Assert.Empty(actualListOfFilteredProduct);
        }
        [Fact]
        public void SearchProductByNameShouldReturnProductDetailsWhenMatchesAreFound()
        {
            var _cut = CreateProduktFilter();
            string productName = "Radio";
            var actualProduct = _cut.SearchProductByName(productName);

            Assert.Equal("2", actualProduct.First().Id);
            Assert.Equal("Radio",actualProduct.First().Name);
            Assert.Equal(250, actualProduct.First().Price);


        }
        [Fact]
        public void SearchProductByNameShouldReturnAnEmptyListWhenNoMatchesAreFound()
        {
            var _cut = CreateProduktFilter();
            var productName = "Kokvåg";
            var actualProduct = _cut.SearchProductByName(productName);

            Assert.False(actualProduct.Any());


        }
        [Fact]
        public void SearchShopByNameShouldReturnShopDetailsWhenMatchesAreFound()
        {
            var _cut = CreateProduktFilter();
            var shopName = "Malmö";
            var actualShop = _cut.SearchShopByName(shopName);

            Assert.Equal("2", actualShop.First().Id);

        }
        [Fact]
        public void SearchShopByNameShouldReturnAnEmptyListWhenNoMatchesAreFound()
        {
            var _cut = CreateProduktFilter();
            var shopName = "Uddevalla";
            var actualShop = _cut.SearchShopByName(shopName);

            Assert.False(actualShop.Any());

        }
        [Fact]
        

    }
}
