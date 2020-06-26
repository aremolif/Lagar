using System.Collections.Generic;
using System.Text;
using Xunit;
using Inlämning5.Classes.Filters;
using Inlämning5.Classes;
using Moq;

namespace Inlämning5.Tests
{

    public class ActionClassShould
    {
        [Fact]
        public void Action_ConstructorExpectsInstantiation()
        {
            
            IButikRepository<Butik> shopsList = new List<Butik>(){
                new Butik("Stockholm"),
                new Butik("Orust"),
                new Butik("Göteborg"),
                new Butik("Malmö")
            };
            ProduktFilter produktQuery = new ProduktFilter(productsList, shopsList);
            _sut = new Actions(productsList,shopsList,produktQuery);
        }
    
    
        
        [Fact]
        public void UpdateShopCollectionShoulReturnAButikObject()
        {
            var _sut = new Actions();
            var _productFilter= new ProduktFilter();

            //IEnumerable<Butik> shopsList = 
        
        }

    }
}
