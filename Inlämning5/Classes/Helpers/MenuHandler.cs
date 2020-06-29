using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Inlämning5.Classes.Repositories;
using Inlämning5.Classes.Filters;
using Inlämning5.Classes.Helpers;

namespace Inlämning5.Classes
{
    public class MenuHandler
    {
        public void RunMenu()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile("appsettings.dev.json", true, true)
                    .Build();

            var mongoConnectionString = configuration["MongoConnection:ConnectionString"];

            var _client = new MongoClient(mongoConnectionString);
            var _database = _client.GetDatabase("Warehouse");
            var _collectionProducts = _database.GetCollection<Product>("Products");
            var _collectionShops = _database.GetCollection<Shop>("Shops");

            //mettere check se mongodb risponde

            IProduktRepository productRepo = new MongoDbProductsRepository(_collectionProducts);
            IButikRepository shopRepo = new MongoDbShopsRepository(_collectionShops);
            EntitiesHelper entitiesHelper = new EntitiesHelper(productRepo, shopRepo);
            //SearchHandler distanceCounter = new SearchHandler();
            ProductFilters productQuery = new ProductFilters(productRepo);

            Actions action = new Actions(entitiesHelper);
            bool endloop = false;
            while (!endloop)
            {
                ConsoleHelper.DisplayMenu();
                try
                {
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            action.AddNewProduct();
                            break;
                        case 2:
                            Console.Clear();
                            action.UpdateProductName();
                            break;
                        case 3:
                            Console.Clear();
                            action.UpdateProductPrice();
                            break;
                        case 4:
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
                            action.RemoveShop();
                            break;
                        case 8:
                            Console.Clear();
                            action.FindProductAvailability();
                            break;
                        case 9:
                            Console.Clear();
                            productQuery.GetProductToSearch();
                            break;
                        case 10:
                            Console.Clear();
                            productQuery.GetMaxPriceToCompare();
                            break;
                        case 11:
                            productQuery.GetManufacturersInventory();
                            break;
                        case 12:
                            ConsoleHelper.PrintProductsList(entitiesHelper.GetAllStock());
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




    }
    
}

