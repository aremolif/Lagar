using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;


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
        public static Product CreateNewProduct(string name)
        {
            Console.WriteLine("Please enter Price (,): ");
            var price = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Please enter manufacturer name: ");
            
            var manufacturer = new Manufacturer() { Name = Console.ReadLine()};
            
            
            Product product = new Product()
            {
                Name = name,
                Manufacturer = manufacturer,
                Price = price
            };
            return product;
        }
        public static string GetProductName()
        {
            Console.WriteLine("Insert product name:");
            var productName = Console.ReadLine();
            return productName;
        }
        public static decimal GetProductPrice()
        {
            Console.WriteLine("Insert new product price:");
            var price = int.Parse(Console.ReadLine());
            return price;
        }
        public static void PrintProductFilteredByPrice(IEnumerable<Product> filteredProducts)
        {
            if (filteredProducts.Any())
            {
                Console.WriteLine($"\n{"Product",-20}  {"Price",10}");
                foreach (var product in filteredProducts)
                    Console.WriteLine($"{product.Name.PadRight(20)}  {product.Price,10}");
                Console.WriteLine("-----");
            }
            else
                Console.WriteLine("  >No matches found");
            
        }
        public static void PrintProductsList(IEnumerable<Product> stockList)
        {
            foreach (var p in stockList)
            {
                Console.WriteLine(p);
                foreach (var b in p.Shops)
                    Console.WriteLine($"  >{b.Name}");
            }
            Console.WriteLine("-----");
        }
        public static void PrintShopsWithinProduct(Product product)
        {
            Console.WriteLine("Current availability");
            foreach (var butik in product.Shops)
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
        public static void PrintFuzzySearchResults(IEnumerable<SearchHandler> searchResults)
        {
            Console.WriteLine("Search results:");
            foreach (var s in searchResults)
                Console.WriteLine($">  {s.MatchedName}");

        }
        public static IEnumerable<Shop> GetShopsWithinProduct(Product product)
        {
            return product.Shops;
        }
    }
    


}
