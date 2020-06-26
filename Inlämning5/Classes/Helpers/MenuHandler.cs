using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Inlämning5.Classes.Repositories;
using Inlämning5.Classes.Filters;
using System.Collections.Generic;

namespace Inlämning5.Classes
{
    public class MenuHandler
    {
        
        public void RunMenu()
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
                ProduktFilter produktQuery = new ProduktFilter(productRepo, shopRepo);

                Actions action = new Actions(productRepo,shopRepo,produktQuery);
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
                            case 1:
                                Console.Clear();
                                action.AddNewProduct();
                                break;
                            case 2:  //ok
                                Console.Clear();
                                action.UpdateProductName();
                                break;
                            case 3: 
                                Console.Clear();
                                action.UpdateProductPrice();
                                break;
                            case 4:  // ok
                                Console.Clear();
                                action.AddShopAvailability();
                                break;
                            case 5: 
                                Console.Clear();
                                action.RemoveShopAvailability();
                                break;
                            case 6:
                                Console.Clear();
                                action.RemoveProduct();
                                break;
                            case 7:
                                Console.Clear();
                                action.RemoveShopFromStock();
                                break;
                            case 8:
                                Console.Clear();
                                action.ListShopFromStock();
                                break;
                            case 9:
                                Console.Clear();
                                action.SearchProduct();
                                break;
                            case 10:
                                Console.Clear();
                                action.SearchProductByPrice();
                                break;
                            case 11:
                                produktQuery.GetManufacturersInventory();
                                break;
                            case 12:  
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
        public string SetPath(string inputFileName)
        {
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var inputPath = Path.Combine(userPath, "InputFiles");
            var filePath = Path.Combine(inputPath, inputFileName);
            return filePath;
        }
    }
}
