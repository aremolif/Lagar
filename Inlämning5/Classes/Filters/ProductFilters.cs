using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes.Filters;

namespace Inlämning5.Classes
{
    public class ProductFilters
    {
        private IProduktRepository ProductRepository { get; }
        private SearchHandler DistanceCounter { get; }

        public ProductFilters(IProduktRepository productRepository, SearchHandler distanceCounter)
        {
            ProductRepository = productRepository;
            DistanceCounter = distanceCounter;

        }
        public IEnumerable<Product> SearchByPrice(string price)
        {
            var maxPrice = decimal.Parse(price);
            if (maxPrice > 0)
                return ProductRepository.GetAll().Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);

            //ConsoleHelper.PrintProductFilteredByPrice(maxPrice, SearchByPrice(maxPrice));
            else
                throw new FormatException();
        }
        public IEnumerable<SearchHandler> SearchByLikelihood(string searchString)
        {
            var products = ProductRepository.GetAll();
            var num = products.Count();
            var distanceList = new List<SearchHandler>();
            var customThreshold = 0.28;
            foreach (var p in products)
            {
                
                DistanceCounter.Distance = DistanceCounter.GetDistance(searchString, p.Name);
                DistanceCounter.MatchedName = p.Name;
                distanceList.Add(DistanceCounter);
            }
            var searchResults = distanceList.Where(d => d.Distance > customThreshold)
                                            .OrderByDescending(x => x.Distance);

            return searchResults;
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
            Console.WriteLine("Insert a product to search");
            var wordToSearch = Console.ReadLine();

            var results = SearchByLikelihood(wordToSearch);
            if (results.Any())
                ConsoleHelper.PrintFuzzySearchResults(results);
            else
                Console.WriteLine("No match found");

            ;
        }
        public void GetMaxPriceToCompare()
        {
            Console.WriteLine("Insert max price");
            var maxPrice = Console.ReadLine();
            try
            {
                var matches = SearchByPrice(maxPrice);
                ConsoleHelper.PrintProductFilteredByPrice(matches);
            }  
            catch (FormatException)
            {
                Console.WriteLine("Price must be a value > 0");
            }
        }

    }
}
