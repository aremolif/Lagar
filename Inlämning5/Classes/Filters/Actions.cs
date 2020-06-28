using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inlämning5.Classes.Helpers;
using MongoDB.Bson;
using Nancy.ViewEngines;

namespace Inlämning5.Classes.Filters
{
    public class Actions
    {
        //private ProductFilters ProductQuery { get; }
        private EntitiesHelper EntitiesHelper { get; }
        public Actions(EntitiesHelper entitiesHelper)
        {
            EntitiesHelper = entitiesHelper;
        }
        //public Actions(ProductFilters productQuery)
        //{
        //    ProductQuery = productQuery;
        //}
        public void AddNewProduct()
        {
            string productName = ConsoleHelper.GetProductName();
            if (EntitiesHelper.SearchProductByName(productName).Any())
                Console.WriteLine($"Product {productName} already present. Press 2 to update in main menu");
            else
            {
                var newProduct = ConsoleHelper.CreateNewProduct(productName);
                Console.WriteLine("Insert shops - type exit to finish:");
                EntitiesHelper.AddShopsToProduct(newProduct);
                EntitiesHelper.AddProductToCollection(newProduct);

            }
        }
        public void UpdateProductName()
        {
            ConsoleHelper.PrintList("Products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            string productName = ConsoleHelper.GetProductName();
            try
            {
                var product = new Product();
                product.Name = productName;
                EntitiesHelper.UpdateProductName(product);
                ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("  >Product not found"); ;
            }
         
        }
        public void UpdateProductPrice()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            string productName = ConsoleHelper.GetProductName();
            try
            {
                var product = new Product();
                product.Name = productName;
                product.Price = ConsoleHelper.GetProductPrice();
                EntitiesHelper.UpdateProductPrice(product);
                ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("  >Product not found");;
            }
        }
        public void RemoveProduct()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name:");
            var productToDelete = Console.ReadLine();
            try
            {
                EntitiesHelper.RemoveProductFromCollection(productToDelete);
                ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Product not found");
            }

        }
        
        public void GetShopsListFromStock()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productToCheck = Console.ReadLine();
            var matchedProducts = EntitiesHelper.SearchProductByName(productToCheck);

            if (matchedProducts.Any())
            {
                foreach (var product in matchedProducts)
                    ConsoleHelper.PrintShopsWithinProduct(product);
            }
            else
                Console.WriteLine($"  >Product {productToCheck} not found"); ;
        }
        public void GetShopToRemove()
        {
            Console.WriteLine("Insert shop name");
            string shopName = Console.ReadLine();
            EntitiesHelper.RemoveShopFromCollection(shopName);

        }
        public void AddShopAvailability()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productAvailable = Console.ReadLine();
            var matches = EntitiesHelper.SearchProductByName(productAvailable);
            if (matches.Any())
            {
                var product = matches.First();
                ConsoleHelper.PrintShopsWithinProduct(product);
                Console.WriteLine("Insert shops - type exit to finish:");
                while (true)
                {
                    var butikName = Console.ReadLine();
                    if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (product.Shops.Count > 0))
                        break;
                    else
                    {
                        if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            EntitiesHelper.UpdateShopsWithinProduct(product, butikName);
                          
                    }
                }
                EntitiesHelper.UpdateExistingProductInCollection(product);
            }
            else
                Console.WriteLine("Product not found"); ;
        }
        public void RemoveShopAvailability()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productToUpdate = Console.ReadLine();
            var matches = EntitiesHelper.SearchProductByName(productToUpdate);
            if (matches.Any())
            {
                var product = matches.First();
                ConsoleHelper.PrintShopsWithinProduct(product);
                Console.WriteLine("Insert shop to remove from product:");
                var shopToDelete = Console.ReadLine();

                var shopMatches = EntitiesHelper.SearchShopByName(shopToDelete);
                if (shopMatches.Any())
                {
                    product.RemoveShop(shopMatches.First());
                    EntitiesHelper.UpdateExistingProductInCollection(product);
                    
                }
                else
                    Console.WriteLine("Shop not found");
            }
            else
                Console.WriteLine("Product not found");

        }
        
    }

}
