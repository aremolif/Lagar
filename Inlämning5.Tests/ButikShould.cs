using System;
using System.Collections.Generic;
using System.Text;
using Inlämning5.Classes;
using Xunit;

namespace Inlämning5.Tests
{
    public class ButikShould
    {
        [Fact]
        void NamePropertyShouldBeInitializedWhenNew()
        {
            var ShopName = "Stockholm";
            Butik _sut = new Butik(ShopName);

            Assert.Equal(ShopName, _sut.Name);
        }
        [Fact]
        void IdPropertyShouldBeNullByDefault()
        {
            Butik _sut = new Butik("Stockholm");

            Assert.Null(_sut.Id);
        }
    
    
    }
}
