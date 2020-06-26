using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes;
using Inlämning5.Tests;
using Xunit;

namespace Inlämning5UnitTests
{
    public class ProduktFilterTest
    {
        IEnumerable<Produkt> CreateTestProducts()
        {
            List<Produkt> products = new List<Produkt>() {
                new Produkt(){
                    Name = "Mixer",
                    Butik = new List<Butik>() { new Butik("Stockholm"), new Butik("Orust"), new Butik("Malmö") },
                    Price = 390
                },
                new Produkt(){
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
                new Butik("Stockholm"),
                new Butik("Malmö"),
                new Butik("Göteborg"),
                new Butik("Orust")
            };
            return shops;
        }
        ProduktFilter CreateProduktFilter()
        {
            var productsRepo = new FakeProductsRepository(CreateTestProducts());
            var shopsRepo = new FakeShopsRepository(CreateTestShops());
            return new ProduktFilter(productsRepo, shopsRepo);
        }
        //{
        //

        //for (int i = 0; i< 101; i++) {

        //    Dinner sampleDinner = new Dinner()
        //    {
        //        //    var repository = new FakeDinnerRepository(CreateTestDinners());
        //        //    return new DinnersController(repository);
                [Fact]
        public void SearchByPriceShouldReturnAListOfPoductsFilteredByPrice()
        {

            IEnumerable<Produkt> _productsList = new List<Produkt>()
            {
                new Produkt()
                {
                    Name= "Mixer",
                    Butik = new List<Butik>(){ new Butik("Stockholm"),new Butik("Orust"), new Butik("Malmö") },
                    Price = 390
                },
                new Produkt()
                {
                    Name="Radio",
                    Butik=new List<Butik>(){new Butik("Göteborg"),new Butik("Stockholm") },
                    Price = 250
                }
            };
            var productList = new FakeProductsRepository();
            
            var _cut = new ProduktFilter(productsList, shopsList);

            var maxPrice = 350;

            var actualListOfFilteredProduct = _cut.SearchByPrice(maxPrice);
            Assert.Single(actualListOfFilteredProduct);
        }

        
    }
}
