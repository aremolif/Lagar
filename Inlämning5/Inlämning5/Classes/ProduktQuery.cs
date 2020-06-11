using System;
using System.Collections.Generic;
using System.Linq;

namespace Inlämning4.Classes
{
    public class ProduktQuery
    {
        internal void SearchProductAvailability(Produkt product)
        {
            if (product.Butik.Count() > 0)
            {
                Console.WriteLine($"Product availability:");
                foreach (var shop in product.Butik)
                    Console.WriteLine($"  >{shop.Name}");
                Console.WriteLine("-----");
            }
            else
                Console.WriteLine($"Product currently not available");
        }
        internal void ChangeProductAvailability(Produkt product)
        {
            Console.WriteLine("Current availability");
            foreach (var butik in product.Butik)
                Console.WriteLine($"  >{butik.Name}");
            Console.WriteLine("Insert shop name");
            string shopDetail = Console.ReadLine();
            Butik newButik = new Butik() { Name = shopDetail };
            Console.WriteLine("Insert the action to perform:");
            Console.WriteLine("1. To add the product to the shop");
            Console.WriteLine("2. To remove the product from the shop ");
            try
            {
                var shopAction = int.Parse(Console.ReadLine());
                var shopChecked = product.Butik.First(s => s.Name == shopDetail);
                switch (shopAction)
                {
                    case 1:
                        if (shopChecked == null)
                            product.Butik.Add(newButik);
                        else
                            Console.WriteLine("Shop already registrered");
                        break;
                    case 2:
                        if (shopChecked != null)
                            product.Butik.Remove(shopChecked);
                        else
                            Console.WriteLine("Shop not found");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Try again");
            }
        }
        internal void SearchByPrice(decimal maxPrice, IEnumerable<Produkt> products)
        {
            var query = products.Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);
            if (!query.Any())
                Console.WriteLine($"No products found with price less than {maxPrice}");
            else
            {
                Console.WriteLine($"\n{"Product",-20}  {"Price",10}");
                foreach (var q in query)
                    Console.WriteLine($"{q.Name.PadRight(20)}  {q.Price,10}");
                Console.WriteLine("-----");
            }
        }
        internal void GetManufacturersInventory(IEnumerable<Produkt> products)
        {
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
        internal void SearchByLikelihood(string searchString, IEnumerable<Produkt> products)
        {
            var distanceList = new List<SearchHandler>();
            foreach (var p in products)
            {
                var distanceCounter = new SearchHandler();
                distanceCounter.distance = distanceCounter.GetDistance(searchString, p.Name);
                distanceCounter.matchedName = p.Name;
                distanceList.Add(distanceCounter);
            }
            var searchResults = distanceList.Where(d => d.distance > 0.28)
                                            .OrderByDescending(x => x.distance);
            Console.WriteLine("Search results:");
            if (searchResults.Any())
            {
                foreach (var s in searchResults)
                    Console.WriteLine($">  {s.matchedName}");
            }
            else
                Console.WriteLine(">  No match found");
        }
    }
}
