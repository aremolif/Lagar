using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inlämning5.Classes;
using Xunit;

namespace Inlämning5.Tests.Models
{
    public class ProductTest
    {
        [Fact]
        public void AddShopShouldUpdateTheListOfShops()
        {
            var _cut = new Product() { Id="1", Name="Mixer", Manufacturer = new Manufacturer() { Name = "Braun"} };

            Shop shop = new Shop() { Id = "10", Name = "Stockholm" };
            _cut.AddShop(shop);

            Assert.NotNull(_cut.Shops);
            Assert.Contains("Stockholm", _cut.Shops.First().Name);
        }
    }
}
