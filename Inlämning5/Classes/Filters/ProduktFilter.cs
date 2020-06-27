using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes.Filters;

namespace Inlämning5.Classes
{
    public class ProduktFilter
    {
        private IProduktRepository ProductRepository { get; }
        private IButikRepository ShopRepository { get; }

        public ProduktFilter(IProduktRepository productRepository, IButikRepository shopRepository)
        {
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
        }
        

        public IEnumerable<Produkt> SearchByPrice(decimal maxPrice)
        {
            return ProductRepository.GetAll().Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);
            
        }
        public IEnumerable<Produkt> SearchProductByName(string name)
        {
            return ProductRepository.GetAll().Where(p=> p.Name.Equals(name));
            
        }
        public IEnumerable<Butik> SearchShopByName(string name)
        {
            return ShopRepository.GetAll().Where(p => p.Name.Equals(name)); 
        }
        public IEnumerable<Produkt> SearchAllStock()
        {
            return ProductRepository.GetAll();
        }
        public IEnumerable<Butik> ListShopsWithProduct(Produkt product)
        {
            return product.Butik;
        }
        public void GetManufacturersInventory()
        {

            var products = ProductRepository.GetAll();
            

            var manufactures = products.SelectMany(p => p.Butik, (manufacture, butik) => new
            {
                manufacture,
                butik
            })
                                        .GroupBy(m => m.manufacture.Tillverkare.Name)
                                        .Select(manufacture => new
                                        {
                                            TillverkareName = manufacture.Key,
                                            ProductCount = manufacture.Count()
                                        })
                                         .OrderBy(m => m.TillverkareName);

            Console.WriteLine($"\n{"Manufacturers",-20}  {"Tot. products",10}");
            foreach (var group in manufactures)
                Console.WriteLine("{0,-20} {1, 10}", group.TillverkareName, group.ProductCount);
            Console.WriteLine("-----");
        }
        public IEnumerable<SearchHandler> SearchByLikelihood(string searchString, IEnumerable<Produkt> products)
        {
            var distanceList = new List<SearchHandler>();
            foreach (var p in products)
            {
                var distanceCounter = new SearchHandler();
                distanceCounter.Distance = distanceCounter.GetDistance(searchString, p.Name);
                distanceCounter.MatchedName = p.Name;
                distanceList.Add(distanceCounter);
            }
            var searchResults = distanceList.Where(d => d.Distance > 0.28)
                                            .OrderByDescending(x => x.Distance);

            return searchResults;
        }
        
    }
}
