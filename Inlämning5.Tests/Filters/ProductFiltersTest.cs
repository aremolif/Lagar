using System;
using System.Collections.Generic;
using System.Text;
using Inlämning5.Classes;
using Xunit;

namespace Inlämning5.Tests.Filters
{
    public class ProductFiltersTest
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
                },
                new Product(){
                    Id = "3",
                    Name = "GlasVåg",
                    Manufacturer= new Manufacturer(){ Name = "Beurer"},
                    Shops = new List<Shop>() { new Shop("Göteborg"), new Shop("Stockholm") },
                    Price = 250
                },
                new Product(){
                    Id = "4",
                    Name = "Kokväg",
                    Manufacturer= new Manufacturer(){ Name = "Tristar"},
                    Shops = new List<Shop>() { new Shop("Göteborg"), new Shop("Stockholm") },
                    Price = 250
                }
            };
            return products;
        }
        ProductFilters CreateProductFilters()
        {
            var productsRepo = new FakeProductsRepository(CreateTestProducts());
            var distanceCounter = new SearchHandler();
            
            return new ProductFilters(productsRepo, distanceCounter);
        }
        [Fact]
        public void SearchByPriceShouldReturnAnEmptyListOfPoductsFilteredByPriceWhenNoMatchesAreFound()
        {

            var _cut = CreateProductFilters();

            var maxPrice = "100";

            var actualListOfFilteredProduct = _cut.SearchByPrice(maxPrice);
            Assert.Empty(actualListOfFilteredProduct);
        }
        [Fact]
        public void SearchByPriceShouldReturnAFormatExceptionWhenInputParseIsNotAValidPriceValue()
        {
            var _cut = CreateProductFilters();
            
            var maxPrice1 = "-100";
            var maxPrice2 = "maxPrice";
 
            Assert.Throws<FormatException>(() => _cut.SearchByPrice(maxPrice1));
            Assert.Throws<FormatException>(() => _cut.SearchByPrice(maxPrice2));
        }
        [Fact]
        public void SearchByNameLikelihoodShouldReturnAnEmptyListWhenDistanceIsLessThanCustomThreshold()
        {
            var _cut = CreateProductFilters();
            var productName = "Stabmixer";
            var actualListOfMatchedName = _cut.SearchByLikelihood(productName);
            Assert.Empty(actualListOfMatchedName);

        }
        [Fact]
        public void SearchByNameLikelihoodShouldReturnAListOfMatchedProductNamesWhenDistanceIsLessThanCustomThreshold()
        {
            var _cut = CreateProductFilters();
            var productName = "våg";
            var actualListOfMatchedName = _cut.SearchByLikelihood(productName);
            Assert.Empty(actualListOfMatchedName);

        }
    }
}
