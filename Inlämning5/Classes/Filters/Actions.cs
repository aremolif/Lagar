using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.ViewEngines;

namespace Inlämning5.Classes.Filters
{
    public class Actions
    {
        //private IProduktRepository ProductRepository { get; }
        //private IButikRepository ShopRepository { get; }
        private ProduktFilter ProductQuery { get; }
        public Actions(ProduktFilter productQuery)
        {
            ProductQuery = productQuery;
        }
        public void AddNewProduct()
        {
            Console.WriteLine("Insert product name:");
            var newProductName = Console.ReadLine();
            if (ProductQuery.SearchProductByName(newProductName).Any())
                Console.WriteLine($"Product {newProductName} already present. Press 2 to update in main menu");
            else
            {
                var newProduct = ConsoleHelper.CreateNewProduct(newProductName);
                Console.WriteLine("Insert shops - type exit to finish:");
                while (true)
                {
                    var butikName = Console.ReadLine();
                    if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (newProduct.Butik.Count > 0))
                        break;
                    else
                    {
                        if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            ProductQuery.UpdateProductAvailability(newProduct, butikName);
                        
                    }
                }
                ProductQuery.AddProductToCollection(newProduct);
                
            }
        }
        public void UpdateProductName()
        {
            ConsoleHelper.PrintList("Products", ProductQuery.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert current product name:");
            var productName = Console.ReadLine();
            var matches = ProductQuery.SearchProductByName(productName);
            if (matches.Any())
            {
                var productToUpdate = matches.First();
                Console.WriteLine("Insert new product name:");
                var newName = Console.ReadLine();
                productToUpdate.Name = newName;
                ProductQuery.UpdateExistingProductInCollection(productToUpdate);
                ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            }
            else
                Console.WriteLine("  >Product not found");
        }
        public void UpdateProductPrice()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert current product name:");
            var Name = Console.ReadLine();
            try
            {
                var product = new Product();
                product.Name = Name;
                Console.WriteLine("Insert new product price:");
                product.Price = int.Parse(Console.ReadLine()); ;
                ProductQuery.UpdateExistingProductInCollection(product);

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Product not found");;
            }
        }
        public void RemoveProduct()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name:");
            var productToDelete = Console.ReadLine();
            try
            {
                ProductQuery.RemoveProductFromCollection(productToDelete);
                ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Product not found");
            }

        }
        public void GetMaxPriceToCompare()
        {
            Console.WriteLine("Insert max price");
            try
            {
                var maxPrice = decimal.Parse(Console.ReadLine());
                if (maxPrice > 0)
                    ConsoleHelper.PrintProductFilteredByPrice(maxPrice, ProductQuery.SearchByPrice(maxPrice));
                else
                    throw new FormatException();
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input");
            }
        }
        public void GetProductToSearch()
        {
            Console.WriteLine("Insert a product to search");
            var wordToSearch = Console.ReadLine();
            
            var results = ProductQuery.SearchByLikelihood(wordToSearch);
            if (results.Any())
                ConsoleHelper.PrintFuzzySearchResults(results);
            else
                Console.WriteLine("No match found");

            ;
        }
        public void GetShopsListFromStock()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productToCheck = Console.ReadLine();
            var matchedProducts = ProductQuery.SearchProductByName(productToCheck);

            if (matchedProducts.Any())
            {
                foreach (var p in matchedProducts)
                    ConsoleHelper.PrintButiker(ProductQuery.GetShopsWithProduct(p));
            }
            else
                Console.WriteLine($"  >Product {productToCheck} not found"); ;
        }
        public void GetShopToRemove()
        {
            Console.WriteLine("Insert shop name");
            string shopName = Console.ReadLine();
            ProductQuery.RemoveShopFromCollection(shopName);

        }
        public void AddShopAvailability()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productAvailable = Console.ReadLine();
            var matches = ProductQuery.SearchProductByName(productAvailable);
            if (matches.Any())
            {
                var product = matches.First();
                ConsoleHelper.PrintShopsWithProduct(product);
                Console.WriteLine("Insert shops - type exit to finish:");
                while (true)
                {
                    var butikName = Console.ReadLine();
                    if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (product.Butik.Count > 0))
                        break;
                    else
                    {
                        if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            ProductQuery.UpdateProductAvailability(product, butikName);
                          
                    }
                }
                ProductQuery.UpdateExistingProductInCollection(product);
            }
            else
                Console.WriteLine("Product not found"); ;
        }
        public void RemoveShopAvailability()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductQuery.GetAllStock().Select(p => p.Name));
            Console.WriteLine("Insert product name");
            string productToUpdate = Console.ReadLine();
            var matches = ProductQuery.SearchProductByName(productToUpdate);
            if (matches.Any())
            {
                var product = matches.First();
                ConsoleHelper.PrintShopsWithProduct(product);
                Console.WriteLine("Insert shop to remove from product:");
                var shopToDelete = Console.ReadLine();

                var shopMatches = ProductQuery.SearchShopByName(shopToDelete);
                if (shopMatches.Any())
                {
                    product.RemoveShop(shopMatches.First());
                    ProductQuery.UpdateExistingProductInCollection(product);
                    
                }
                else
                    Console.WriteLine("Shop not found");
            }
            else
                Console.WriteLine("Product not found");

        }

       
    }

}
