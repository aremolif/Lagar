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
        internal void DisplayMenu()
        {
            Console.WriteLine("Choose an action from the menu:");
            Console.WriteLine("_______________________________");
            Console.WriteLine(" 1.  Add a product");
            Console.WriteLine(" 2.  Update a product");
            Console.WriteLine(" 3.  Delete a product");
            Console.WriteLine(" 4.  Find product availability");
            Console.WriteLine(" 5.  Modify product availability");
            Console.WriteLine(" 6.  Search products by likelihood");
            Console.WriteLine(" 7.  Search products by price threshold");
            Console.WriteLine(" 8.  List products available by manufacturers");
            Console.WriteLine(" 9.  Remove a shop from stock");
            Console.WriteLine(" 10. List the whole stock");
            Console.WriteLine(" 0.  Exit the program");
        }
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
                    DisplayMenu();
                    try
                    {
                        int choice = int.Parse(Console.ReadLine());
                        var product = new Produkt();
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Insert product name:");
                                var newProductName = Console.ReadLine();
                                if (productRepo.GetById(newProductName) != null)
                                    Console.WriteLine($"Product {newProductName} already present. Press 2 to update in main menu");
                                else
                                {
                                    product = GetProductDetails();
                                    Console.WriteLine("Insert shops - type exit to finish:");
                                    AddListButik(product);

                                    productRepo.Insert(product);
                                    productRepo.Save();
                                }
                                break;
                            case 2:
                                Console.WriteLine("Insert product name:");
                                var productCurrentName = Console.ReadLine();
                                var productToUpdate = productRepo.GetById(productCurrentName);
                                if (productToUpdate != null)
                                {
                                    product = GetProductDetails();
                                    Console.WriteLine("Insert shops - type exit to finish:");
                                    
                                    AddListButik(product);

                                    productRepo.Save();
                                }
                                else
                                    Console.WriteLine($"Product {productToUpdate} not found. Press 1 to add in main menu");
                                break;
                            case 3:
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
                            case 4:
                                
                                Console.WriteLine("Insert product name");
                                string productName = Console.ReadLine();
                                var matchedProduct = produktQuery.SearchProductByName(productName);

                                if (matchedProduct.Any())
                                {
                                    foreach (var p in matchedProduct)
                                        ConsoleHelper.PrintButiker(produktQuery.ListShopsWithProduct(p));
                                }
                                else
                                    Console.WriteLine($"  >Product {productName} not found"); ;
                                break;
                            case 5:
                                Console.WriteLine("Insert product name");
                                string productAvailable = Console.ReadLine();
                                product = productRepo.GetById(productAvailable);
                                produktQuery.ChangeProductAvailability(product);
                                break;
                            case 6:
                                Console.WriteLine("Insert a product to search");
                                var wordToSearch = Console.ReadLine();
                                var productsStock = productRepo.GetAll();
                                var results = produktQuery.SearchByLikelihood(wordToSearch, productsStock);
                                if (results.Any())
                                    produktQuery.PrintFuzzySearchResults(results);
                                else
                                    Console.WriteLine("No match found");
                                
                                break;
                            case 7:
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
                            case 8:
                                var products = productRepo.GetAll();
                                produktQuery.GetManufacturersInventory(products);
                                break;
                            case 9:
                                Console.WriteLine("Insert shop name");
                                string shopToRemove = Console.ReadLine();
                                var productsList = productRepo.GetAll();
                                foreach (var productToCheck in productsList)
                                {
                                    var shopIndex = productToCheck.Butik.First(s => s.Name == shopToRemove);
                                    productToCheck.Butik.Remove(shopIndex);
                                }
                                break;
                            case 10:  //not required
                                
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

        

        private static Produkt GetProductDetails()
        {
            var product = new Produkt();
            Console.WriteLine("Insert product price:");
            product.Price = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert product manufacturer:");
            product.Tillverkare.Name = Console.ReadLine();
            return product;
        }

        private static void AddListButik(Produkt product)
        {
            while (true)
            {
                var butikToAdd = new Butik();
                var butikName = Console.ReadLine();

                if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (product.Butik.Count > 0))
                    break;
                else
                {
                    if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        butikToAdd.Name = butikName;
                        product.Butik.Add(butikToAdd);
                    }
                }
            }
        }
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
