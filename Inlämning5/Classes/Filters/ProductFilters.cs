using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes.Filters;

namespace Inlämning5.Classes
{
    public class ProductFilters
    {
        private IProductRepository ProductRepository { get; }
        public ProductFilters(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }
        public IEnumerable<Product> SearchByPrice(string price)
        {
            var maxPrice = decimal.Parse(price);
            if (maxPrice > 0)
                return ProductRepository.GetAll().Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);
            else
                throw new FormatException();
        }
        public IEnumerable<SearchHandler> SearchByLikelihood(string searchString)
        {
            var distanceList = new List<SearchHandler>();
            var products = ProductRepository.GetAll();
            var num = products.Count();
            
            double customThreshold = 0.25;
            foreach (var p in products)
            {
                var distanceCounter = new SearchHandler();
                distanceCounter.Distance = distanceCounter.GetDistance(searchString, p.Name);
                distanceCounter.MatchedName = p.Name;
                distanceList.Add(distanceCounter);
            }
            var searchResults = distanceList.Where(d => d.Distance > customThreshold)
                                            .OrderByDescending(x => x.Distance);

            return searchResults;
        }
        public void SearchProductAvailability()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductRepository.GetAll().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productToCheck = Console.ReadLine();
            var matchedProducts = ProductRepository.GetAll().Where(p => p.Name.Equals(productToCheck));

            if (matchedProducts.Any())
            {
                foreach (var product in matchedProducts)
                    ConsoleHelper.PrintList<Shop>("Current availability",product.Shops);
            }
            else
                Console.WriteLine($"  >Product {productToCheck} not found"); ;
        }
        public void GetManufacturersInventory()
        {

            var products = ProductRepository.GetAll();
            var manufactures = products.SelectMany(p => p.Shops, (manufacturer, shop) => new
            {
                manufacturer,
                shop
            })
                                        .GroupBy(m => m.manufacturer.Manufacturer.Name)
                                        .Select(manufacturer => new
                                        {
                                            ManufacturerName = manufacturer.Key,
                                            ProductCount = manufacturer.Count()
                                        })
                                         .OrderBy(m => m.ManufacturerName);

            Console.WriteLine($"\n{"Manufacturers",-20}  {"Tot. products",10}");
            foreach (var group in manufactures)
                Console.WriteLine("{0,-20} {1, 10}", group.ManufacturerName, group.ProductCount);
            Console.WriteLine("-----");
        }
        public void GetProductToSearch()
        {
            Console.WriteLine("Insert product name to search");
            var wordToSearch = Console.ReadLine();

            var results = SearchByLikelihood(wordToSearch);
            if (results.Any())
                ConsoleHelper.PrintList<SearchHandler>("Search results", results);
            else
                Console.WriteLine("No match found");
        }
        public void SearchProductsByPrice()
        {
            var priceToCompare = ConsoleHelper.GetProductPrice();
            var matches = SearchByPrice(priceToCompare);
            ConsoleHelper.PrintProductFilteredByPrice(matches);
        }
    }
}
