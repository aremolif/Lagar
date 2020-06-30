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
            Console.WriteLine(" 4.  Add shop to a product");
            Console.WriteLine(" 5.  Remove shop from a product");
            Console.WriteLine(" 6.  Remove a product from stock");
            Console.WriteLine(" 7.  Remove a shop from stock");
            Console.WriteLine(" 8.  Search product availability");
            Console.WriteLine(" 9.  Search products by likelihood");
            Console.WriteLine(" 10. Search products by price threshold");
            Console.WriteLine(" 11. List products available by manufacturers");
            Console.WriteLine(" 12. List the whole stock");
            Console.WriteLine(" 0.  Exit the program");
        }
        public static Product CreateNewProduct(string name)
        {
            Console.WriteLine("Please enter price: ");
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
            return Console.ReadLine();
            
        }
        public static string GetProductPrice()
        {
            Console.WriteLine("Insert product price:");
            return Console.ReadLine();
        }
        public static string GetShopName()
        {
            return Console.ReadLine();

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
            foreach (var product in stockList)
            {
                Console.WriteLine(product);
                PrintList("", product.Shops);
            }
            Console.WriteLine("-----");
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
