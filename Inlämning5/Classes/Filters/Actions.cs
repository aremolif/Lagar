using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes.Helpers;

namespace Inlämning5.Classes.Filters
{
    public class Actions
    {
        private EntitiesHelper EntitiesHelper { get; }
        public Actions(EntitiesHelper entitiesHelper)
        {
            EntitiesHelper = entitiesHelper;
        }
        public void AddNewProduct()
        {
            string productName = ConsoleHelper.GetProductName();
            if (EntitiesHelper.GetProductByName(productName).Any())
                Console.WriteLine($"Product {productName} already present. Press 2 to update in main menu");
            else
            {
                var product = ConsoleHelper.CreateNewProduct(productName);
                AddShopsToProduct(product);
                EntitiesHelper.AddProductToCollection(product);
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
                Console.WriteLine("Insert new name:");
                string newName = Console.ReadLine();
                EntitiesHelper.UpdateProductName(newName, product);
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
                product.Price = int.Parse(ConsoleHelper.GetProductPrice());
                EntitiesHelper.UpdateProductPrice(product);
                Console.WriteLine("  >Done");
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
        public void RemoveShop()
        {
            Console.WriteLine("Insert shop name");
            string shopName = Console.ReadLine();
            EntitiesHelper.RemoveShopFromCollection(shopName);
        }
        public void UpdateShopsinProduct()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productAvailable = Console.ReadLine();
            var matches = EntitiesHelper.GetProductByName(productAvailable);
            if (matches.Any())
            {
                var product = matches.First();
                ConsoleHelper.PrintList<Shop>("Current availability",product.Shops);
                Console.WriteLine("Insert shops - type exit to finish:");
                AddShopsToProduct(product);
                EntitiesHelper.UpdateProductInCollection(product);
                ConsoleHelper.PrintList<Shop>("Current availability", product.Shops);
            }
            else
                Console.WriteLine("Product not found");
        }
        public void AddShopsToProduct(Product product)
        {
            Console.WriteLine("Insert shops - type exit to finish:");
            while (true)
            {
                var shopName = ConsoleHelper.GetShopName();
                if (shopName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (product.Shops.Count > 0))
                    break;
                else
                    EntitiesHelper.UpdateShopsWithinProduct(product, shopName);
            }
        }
        public void RemoveShopFromProduct()
        {
            ConsoleHelper.PrintList("Warehouse products", EntitiesHelper.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productToUpdate = Console.ReadLine();
            var matches = EntitiesHelper.GetProductByName(productToUpdate);
            if (matches.Any())
            {
                var product = matches.First();
                ConsoleHelper.PrintList<Shop>("Current availability", product.Shops);
                Console.WriteLine("Insert shop to remove from product:");
                var shopToDelete = Console.ReadLine();

                var shopMatches = EntitiesHelper.GetShopByName(shopToDelete);
                if (shopMatches.Any())
                {
                    product.RemoveShop(shopMatches.First());
                    EntitiesHelper.UpdateProductInCollection(product);
                    ConsoleHelper.PrintList<Shop>("Current availability", product.Shops);

                }
                else
                    Console.WriteLine("Shop not found");
            }
            else
                Console.WriteLine("Product not found");
        }
    }
}
