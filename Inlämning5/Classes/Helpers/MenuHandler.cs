using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Inlämning5.Classes.Repositories;
using System.Collections.Generic;

namespace Inlämning5.Classes
{
    public class MenuHandler
    {
        
        internal void RunMenu(string jsonPath)
        {
            try
            {
                

                //***MONGODB
                IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile("appsettings.dev.json", true, true)
                        .Build();

                var mongoConnectionString = configuration["MongoConnection:ConnectionString"];
                                
                var _client = new MongoClient(mongoConnectionString);
                var _database = _client.GetDatabase("Warehouse");
                var _collectionProducts = _database.GetCollection<Produkt>("Products");
                var _collectionShops = _database.GetCollection<Butik>("Shops");


                IProduktRepository productRepo = new MongoDbProduktRepository(_collectionProducts);

                IButikRepository shopRepo = new MongoDbButikRepository(_collectionShops);
                
                
                //var shopsList = shopRepo.GetAll();
                //var totalItems = shopsList.Count();
                
                
                
                ProduktFilter produktQuery = new ProduktFilter(productRepo, shopRepo);


                bool endloop = false;
                while (!endloop)
                {
                    ConsoleHelper.DisplayMenu();
                    try
                    {
                        int choice = int.Parse(Console.ReadLine());
                        var product = new Produkt();
                        switch (choice)
                        {
                            case 1:  //ok
                                Console.WriteLine("Insert product name:");
                                var newProductName = Console.ReadLine();
                                if (produktQuery.SearchProductByName(newProductName).Any())
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
                                                var newButik = new Butik(butikName);
                                                if (!produktQuery.SearchShopByName(butikName).Any())  //negozio non ancora censito
                                                {
                                                    
                                                    shopRepo.Insert(newButik);

                                                }
                                                newButik.Id= produktQuery.SearchShopByName(butikName).First().Id;
                                                newProduct.AddShop(newButik);
                                            }
                                        }
                                    }
                                    productRepo.Insert(newProduct);   
                                }
                                break;
                            case 2:  //ok
                                ConsoleHelper.PrintList("All products", productRepo.GetAll().Select(p => p.Name));
                                Console.WriteLine("Insert current product name:");
                                var productName = Console.ReadLine();
                                var productToUpdate = produktQuery.SearchProductByName(productName).First();
                                Console.WriteLine(productToUpdate.Id);
                                if (productToUpdate != null)
                                {
                                    Console.WriteLine("Insert new product name:");
                                    var newName = Console.ReadLine();
                                    productToUpdate.Name = newName;
                                    productRepo.Update(productToUpdate);
                                    ConsoleHelper.PrintList("All products", productRepo.GetAll().Select(p => p.Name));
                                }
                                else
                                    Console.WriteLine("Product not found");
                                break;
                            case 3: //ok
                                ConsoleHelper.PrintList("All products", productRepo.GetAll().Select(p => p.Name));
                                Console.WriteLine("Insert current product name:");
                                var Name = Console.ReadLine();
                                var productToChange = produktQuery.SearchProductByName(Name).First();
                                
                                if (productToChange != null)
                                {
                                    Console.WriteLine($"Current product price: {productToChange.Price}");
                                    Console.WriteLine("Insert new product price:");
                                    var newPrice = int.Parse(Console.ReadLine()); ;
                                    productToChange.Price = newPrice;
                                    productRepo.Update(productToChange);
                                    //ConsoleHelper.PrintList("Product updated", productRepo.GetById(productToChange.Id).Name);
                                    Console.WriteLine($"price updated: {productToChange.Name} {productToChange.Price}");
                                }
                                else
                                    Console.WriteLine("Product not found");
                                break;
                            case 4:  // ok
                                
                                Console.WriteLine("Insert product name");
                                string productAvailable = Console.ReadLine();
                                product = produktQuery.SearchProductByName(productAvailable).First();
                                if (product != null)
                                {
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
                                                product.AddShop(newButik);  //aggiunge anche se e' giå nel negozio
                                                var butik = produktQuery.SearchShopByName(butikName);
                                                //aggiornare shop DB
                                                if (!butik.Any())
                                                {
                                                    shopRepo.Insert(newButik);
                                                }
                                            }
                                        }
                                    }
                                    //aggiorna il record
                                    productRepo.Update(product);
                                }
                                else
                                    Console.WriteLine("Product not found");
                                break;
                            case 5: //Da verificare****
                                Console.WriteLine("Insert product name");
                                string productToAdjust = Console.ReadLine();
                                product = produktQuery.SearchProductByName(productToAdjust).First();
                                if (product != null)
                                {
                                    Console.WriteLine("Insert shop to remove from product:");
                                    var shopToDelete = Console.ReadLine();
                                    Console.WriteLine(produktQuery.SearchShopByName(shopToDelete).First());
                                    product.RemoveShop(produktQuery.SearchShopByName(shopToDelete).First());
                                    
                                    productRepo.Update(product);
                                }
                                else
                                    Console.WriteLine("Product not found");
                                break;
                            case 6:  //delete product from stock
                                Console.WriteLine("Insert product name:");
                                var productToDelete = Console.ReadLine();
                                product = productRepo.GetById(productToDelete);
                                if (product != null)
                                {
                                    productRepo.Delete(product);
                                    Console.WriteLine("Product deleted\n");
                                }
                                else
                                    Console.WriteLine("Product not found");
                                break;

                            case 7:  //remove shop from stock
                                Console.WriteLine("Insert shop name");
                                string shopToRemove = Console.ReadLine();
                                var productsList = productRepo.GetAll();
                                foreach (var productScan in productsList)
                                {
                                    var shopIndex = productScan.Butik.First(s => s.Name == shopToRemove);
                                    productScan.Butik.Remove(shopIndex);
                                }
                                break;
                            case 8:

                                Console.WriteLine("Insert product name");
                                string productToCheck = Console.ReadLine();
                                var matchedProducts = produktQuery.SearchProductByName(productToCheck);

                                if (matchedProducts.Any())
                                {
                                    foreach (var p in matchedProducts)
                                        ConsoleHelper.PrintButiker(produktQuery.ListShopsWithProduct(p));
                                }
                                else
                                    Console.WriteLine($"  >Product {productToCheck} not found"); ;
                                break;
                            case 9:
                                Console.WriteLine("Insert a product to search");
                                var wordToSearch = Console.ReadLine();
                                var productsStock = productRepo.GetAll();
                                var results = produktQuery.SearchByLikelihood(wordToSearch, productsStock);
                                if (results.Any())
                                    produktQuery.PrintFuzzySearchResults(results);
                                else
                                    Console.WriteLine("No match found");

                                break;

                            case 10:
                                Console.WriteLine("Insert max price");
                                try
                                {
                                    var maxPrice = decimal.Parse(Console.ReadLine());
                                    if (maxPrice > 0)
                                        ConsoleHelper.PrintProductFilteredByPrice(maxPrice, produktQuery.SearchByPrice(maxPrice));
                                    else
                                        throw new FormatException();
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Invalid input");
                                }
                                break;
                            case 11:
                                var products = productRepo.GetAll();
                                //produktQuery.GetManufacturersInventory(products);

                                ConsoleHelper.PrintList("Products for each Manufacturer", produktQuery.ListOfManufacturersWithProductCount());
                                break;
                            case 12:  //not required
                                
                                ConsoleHelper.PrintProductsList(produktQuery.SearchAllStock());
                                break;
                            case 0:
                                endloop = true;
                                break;
                            default:
                                Console.WriteLine("Insert a value between 0 and 10");
                                break;
                        }
                    }
                    catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
                    {
                        Console.WriteLine("Invalid input. Try again");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Input file format error");
            }



        }

        

        

        //private static void AddListButik(Produkt product)
        //{
        //    while (true)
        //    {
        //        var butikToAdd = new Butik();
        //        var butikName = Console.ReadLine();

        //        if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (product.Butik.Count > 0))
        //            break;
        //        else
        //        {
        //            if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
        //            {
        //                butikToAdd.Name = butikName;
        //                product.Butik.Add(butikToAdd);
        //            }
        //        }
        //    }
        //}
        
        private static void PrintButiker(object v)
        {
            throw new NotImplementedException();
        }
        public string SetPath(string inputFileName)
        {
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var inputPath = Path.Combine(userPath, "InputFiles");
            var filePath = Path.Combine(inputPath, inputFileName);
            return filePath;
        }
    }
}
