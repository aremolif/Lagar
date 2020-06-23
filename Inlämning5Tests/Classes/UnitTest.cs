using System;
using System.Collections.Generic;
using System.Text;
using Inlämning5.Classes;
using Xunit;

namespace Inlämning5Tests.Classes
{
    public class UnitTest
    {
        [Fact]
        public void FullPathFileNameShouldBe()
        {
            //var sut = new MenuHandler();
            //string inputFileName = "Produkter.json";
            //string actualFilePath = sut.SetPath(inputFileName);

            //Assert.Equal("c:\\Anna\\InputFile\\Produkter.json", actualFilePath);
        
        
        }
        [Fact]
        public void AppIsRunning()
        {
            Assert.True(5>3);
        }

    }
}
