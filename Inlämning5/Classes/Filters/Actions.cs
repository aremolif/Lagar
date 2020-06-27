using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inlämning5.Classes.Filters
{
    public class Actions
    {
        private IProduktRepository ProductRepository { get; }
        private IButikRepository ShopRepository { get; }
        private ProduktFilter ProductQuery { get; }
        public Actions(IProduktRepository productRepository, IButikRepository shopRepository, ProduktFilter productQuery)
        {
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
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
                        {
                            var newShop=UpdateShopCollection(butikName);
                            newProduct.AddShop(newShop);
                        }
                    }
                }
                ProductRepository.Insert(newProduct);
            }
        }
        public Butik UpdateShopCollection(string shopName)
        {
            var newShop = new Butik(shopName);
            if (!ProductQuery.SearchShopByName(shopName).Any())  //negozio non ancora censito
            {

                ShopRepository.Insert(newShop);

            }
            newShop.Id = ProductQuery.SearchShopByName(shopName).First().Id;
            return newShop;
            
        }
        public void UpdateProductName()
        {
            ConsoleHelper.PrintList("Current product list", ProductRepository.GetAll().Select(p => p.Name));
            Console.WriteLine("Insert current product name:");
            var productName = Console.ReadLine();
            var matches = ProductQuery.SearchProductByName(productName);
            if (matches.Any())
            {
                var productToUpdate = matches.First();
                Console.WriteLine("Insert new product name:");
                var newName = Console.ReadLine();
                productToUpdate.Name = newName;
                ProductRepository.Update(productToUpdate);
                ConsoleHelper.PrintList("All products", ProductRepository.GetAll().Select(p => p.Name));
            }
            else
                Console.WriteLine("  >Product not found");
        }
        public void RemoveProduct()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductRepository.GetAll().Select(p => p.Name));
            Console.WriteLine("Insert product name:");
            var productToDelete = Console.ReadLine();
            var matches = ProductQuery.SearchProductByName(productToDelete);
            if (matches.Any())
            {
                ProductRepository.Delete(matches.First());
                ConsoleHelper.PrintList("All products", ProductRepository.GetAll().Select(p => p.Name));
            }
            else
                Console.WriteLine("Product not found");
            
        }
        public void SearchProductByPrice()
        {
            

            //ConsoleHelper.PrintList("Warehouse products", query);
            
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
        public void SearchProduct()
        {
            Console.WriteLine("Insert a product to search");
            var wordToSearch = Console.ReadLine();
            var productsStock = ProductRepository.GetAll();
            var results = ProductQuery.SearchByLikelihood(wordToSearch, productsStock);
            if (results.Any())
                ConsoleHelper.PrintFuzzySearchResults(results);
            else
                Console.WriteLine("No match found");

            ;
        }
        public void ListShopFromStock()
        {
            ConsoleHelper.PrintList("Warehouse products", ProductRepository.GetAll().Select(p => p.Name));
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
        public void RemoveShopFromStock()  //toglie dai prodotti e dal negozio
        {
            Console.WriteLine("Insert shop name");
            string shopName = Console.ReadLine();
            var shopToRemove = ProductQuery.SearchShopByName(shopName);
            if(shopToRemove.Any())
            {
                ShopRepository.Delete(shopToRemove.First());
            }
            var productsList = ProductRepository.GetAll().ToList();
            foreach (var product in productsList)
            {

                var matches = product.Butik.Where(b => b.Name == shopName);
                if (matches.Any())
                    product.RemoveShop(matches.First());
            }

        }
        public void UpdateProductPrice()
        {
            ConsoleHelper.PrintList("Products in stock", ProductRepository.GetAll().Select(p => p.Name));
            
            Console.WriteLine("Insert current product name:");
            var Name = Console.ReadLine();
            var productToChange = ProductQuery.SearchProductByName(Name).First();

            if (productToChange != null)
            {
                Console.WriteLine($"Current product price: {productToChange.Price}");
                Console.WriteLine("Insert new product price:");
                var newPrice = int.Parse(Console.ReadLine()); ;
                productToChange.Price = newPrice;
                ProductRepository.Update(productToChange);
                Console.WriteLine($"price updated: {productToChange.Name} {productToChange.Price}");
            }
            else
                Console.WriteLine("Product not found"); ;
        }
        public void AddShopAvailability()
        {
            ConsoleHelper.PrintList("Current product list", ProductRepository.GetAll().Select(p => p.Name));
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
                        {
                            var newButik = new Butik(butikName);
                            product.AddShop(newButik);  
                            var butik = ProductQuery.SearchShopByName(butikName);
                            if (!butik.Any())
                                ShopRepository.Insert(newButik);
                        }
                    }
                }
                ProductRepository.Update(product);
            }
            else
                Console.WriteLine("Product not found"); ;
        }
        public void RemoveShopAvailability()
        {
            ConsoleHelper.PrintList("Current product list", ProductRepository.GetAll().Select(p => p.Name));
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
                    ProductRepository.Update(product);
                    //ConsoleHelper.PrintShopsWithProduct(product);
                }
                else
                    Console.WriteLine("Shop not found");
            }
            else
                Console.WriteLine("Product not found");

        }

       
    }

}
