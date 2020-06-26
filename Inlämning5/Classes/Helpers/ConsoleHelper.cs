using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Inlämning5.Classes.Models;

namespace Inlämning5.Classes
{
    public static class ConsoleHelper
    {
        public static void DisplayMenu()
        {
            Console.WriteLine("Choose an action from the menu:");
            Console.WriteLine("_______________________________");
            Console.WriteLine(" 1.  Add a product");
            Console.WriteLine(" 2.  Update product name");
            Console.WriteLine(" 3.  Update product price");
            Console.WriteLine(" 4.  Add product to a shop");
            Console.WriteLine(" 5.  Remove product from a shop");
            Console.WriteLine(" 6.  Remove a product from stock");
            Console.WriteLine(" 7.  Remove a shop from stock");
            Console.WriteLine(" 8.  Find product availability");
            Console.WriteLine(" 9.  Search products by likelihood");
            Console.WriteLine(" 10. Search products by price threshold");
            Console.WriteLine(" 11. List products available by manufacturers");
            Console.WriteLine(" 12. List the whole stock");
            Console.WriteLine(" 0.  Exit the program");
        }
        public static Produkt CreateNewProduct(string name)
        {
            Console.WriteLine("Please enter Price (,): ");
            var price = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Please enter Manufacturer id : ");
            var mID = int.Parse(Console.ReadLine());
            Tillverkare manufacturer = TillverkareService.GetManufacturer(mID);
            
            Produkt product = new Produkt()
            {
                Name = name,
                Tillverkare = manufacturer,
                Price = price
            };
            return product;
        }
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
        public static void PrintList<T>(string header, IEnumerable<T> items)
        {
            Console.WriteLine(header);
            foreach (var item in items)
            {
                Console.WriteLine($"  >{item}");
            }
        }
    }
    


}
