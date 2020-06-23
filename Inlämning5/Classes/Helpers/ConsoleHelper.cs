using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace Inlämning5.Classes
{
    public static class ConsoleHelper
    {
        public static void PrintProductFilteredByPrice(decimal maxPrice, IEnumerable<Produkt> FilteredProdukt)
        {
            if (!FilteredProdukt.Any())
                Console.WriteLine($"No products found with price less than {maxPrice}");
            else
            {
                Console.WriteLine($"\n{"Product",-20}  {"Price",10}");
                foreach (var product in FilteredProdukt)
                    Console.WriteLine($"{product.Name.PadRight(20)}  {product.Price,10}");
                Console.WriteLine("-----");
            }
        }
        public static void PrintProductsList(IEnumerable<Produkt> stockList)
        {
            foreach (var p in stockList)
            {
                Console.WriteLine(p);
                foreach (var b in p.Butik)
                    Console.WriteLine($"  >{b.Name}");
            }
            Console.WriteLine("-----");
        }

        public static void PrintButiker(IEnumerable<Butik> shopList)
        {
            Console.WriteLine($"Product availability:");
            foreach (var butik in shopList)
            {
                Console.WriteLine($"  >{butik.Name}");
            }
        }
        public static void PrintShopsWithProduct(Produkt product)
        {
            Console.WriteLine("Current availability");
            foreach (var butik in product.Butik)
                Console.WriteLine($"  >{butik.Name}"); ;
        }
    }
    


}
