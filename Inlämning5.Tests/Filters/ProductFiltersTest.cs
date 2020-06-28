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
                }
            };
            return products;
        }
        ProductFilters CreateProductFilters()
        {
            var productsRepo = new FakeProductsRepository(CreateTestProducts());
            
            return new ProductFilters(productsRepo);
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
            
            var maxPrice = "-100";
 
            Assert.Throws<FormatException>(() => _cut.SearchByPrice(maxPrice));
        }
    }
}
