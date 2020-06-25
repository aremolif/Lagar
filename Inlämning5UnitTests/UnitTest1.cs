using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes;
using Xunit;

namespace Inlämning5UnitTests
{
    public class Inlämning5ProduktQueryUnitTest
    {
        [Fact]
        public void SearchProductAvailabilityShouldReturnThelistOfShops()
        {
            var sut = new ProduktFilter();
            
            var product = new Produkt()
            {
                Name = "Mixer",
                //Butik = new List<Butik>() { new Butik() { Name = "Stockholm" }, new Butik() { Name = "Orust" }, new Butik() { Name = "Malmö" } }
            };

            var actualShopList = sut.SearchProductAvailability(product);
            
            //ICollection<Butik> expectedShopList = new List<Butik>() { new Butik() { Name = "Stockholm" }, new Butik() { Name = "Orust" }, new Butik() { Name = "Malmö" } };

         //   Assert.Equal(actualShopList.Count(), expectedShopList.Count());

        }
        [Fact]
        public void AddButikShouldReturnANewButikObject()
        {
            var sut = new ProduktFilter();
          //  var shopName = "Stockholm";

           // var actualButik = sut.AddButik(shopName);

          //  Assert.True(actualButik.Name == "Stockholm");
        }

        [Fact]
        public void SearchByLikelihoodShouldReturnOnlyProductNameUnderDistanceThreshold()
        {
            var sut = new ProduktFilter();
            


        }
        [Fact]
        public void SearchByPriceShouldReturnAListOfPoductsFilteredByPrice()
        {

            List<Produkt> _productsList = new List<Produkt>()
            {
                new Produkt()
                {
                    Name= "Mixer",
                    //Butik = new List<Butik>(){ new Butik() { Name = "Stockholm" },new Butik(){Name="Orust"}, new Butik() { Name = "Malmö" } },
                    Price = 390
                },
                new Produkt()
                {
                    Name="Radio",
                 //   Butik=new List<Butik>(){new Butik(){Name="Göteborg" },new Butik(){Name="Stockholm" } },
                    Price = 250
                }
            };
            var sut = new ProduktFilter();
            
            var maxPrice = 350;

            var actualListOfFilteredProduct = sut.SearchByPrice(maxPrice);
            Assert.Single(actualListOfFilteredProduct);
        }
    }
}
