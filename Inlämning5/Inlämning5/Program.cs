using System;
using System.IO;
using System.Linq;
using Inlämning4.Classes;


namespace Inlämning4
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonPath = SetPath();
            if (File.Exists(jsonPath))
            {
                try
                {
                    JsonFileHandler jsonHandler = new JsonFileHandler(jsonPath);
                    IProduktRepository productRepo = new JsonFileRepository(jsonHandler);
                    ProduktQuery produktQuery = new ProduktQuery();
                    bool endloop = false;
                    while (!endloop)
                    {
                        MenuHandler.DisplayMenu();
                        try
                        {
                            int choice = int.Parse(Console.ReadLine());
                            var product = new Produkt();
                            switch (choice)
                            {
                                case 1:
                                    Console.WriteLine("Insert product name:");
                                    var newProductName = Console.ReadLine();
                                    if (productRepo.GetByProductName(newProductName) != null)
                                        Console.WriteLine($"Product {newProductName} already present. Press 2 to update in main menu");
                                    else
                                    {
                                        var newProduct = new Produkt() { Name = newProductName };
                                        product = GetProductDetails(newProduct);
                                        productRepo.Add(product);
                                        productRepo.Save();
                                    }
                                    break;
                                case 2:
                                    Console.WriteLine("Insert product name:");
                                    var productCurrentName = Console.ReadLine();
                                    var productToUpdate = productRepo.GetByProductName(productCurrentName);
                                    if (productToUpdate != null)
                                    {
                                        product = GetProductDetails(productToUpdate);
                                        productRepo.Save();
                                    }
                                    else
                                        Console.WriteLine($"Product {productToUpdate} not found. Press 1 to add in main menu");
                                    break;
                                case 3:
                                    Console.WriteLine("Insert product name:");
                                    var productToDelete = Console.ReadLine();
                                    product = productRepo.GetByProductName(productToDelete);
                                    if (product != null)
                                    {
                                        productRepo.Delete(product);
                                        Console.WriteLine("Product deleted\n");
                                    }
                                    else
                                        Console.WriteLine("Product not found");
                                    break;
                                case 4:
                                    product = new Produkt();
                                    Console.WriteLine("Insert product name");
                                    string productToFind = Console.ReadLine();
                                    product = productRepo.GetByProductName(productToFind);
                                    if (product != null)
                                        produktQuery.SearchProductAvailability(product);
                                    else
                                        Console.WriteLine("Product not found"); ;
                                    break;
                                case 5:
                                    Console.WriteLine("Insert product name");
                                    string productAvailable = Console.ReadLine();
                                    product = productRepo.GetByProductName(productAvailable);
                                    produktQuery.ChangeProductAvailability(product);
                                    break;
                                case 6:
                                    Console.WriteLine("Insert a product to search");
                                    var wordToSearch = Console.ReadLine();
                                    var productsStock = productRepo.GetAll();
                                    produktQuery.SearchByLikelihood(wordToSearch, productsStock);
                                    break;
                                case 7:
                                    Console.WriteLine("Insert max price");
                                    try
                                    {
                                        var maxPrice = decimal.Parse(Console.ReadLine());
                                        if (maxPrice > 0)
                                            produktQuery.SearchByPrice(maxPrice, productRepo.GetAll());
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
            else
                Console.WriteLine("Input file not found");
        }
        private static Produkt GetProductDetails(Produkt product)
        {
            Console.WriteLine("Insert product price:");
            product.Price = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert product manufacturer:");
            product.Tillverkare.Name = Console.ReadLine();
            Console.WriteLine("Insert shops - type exit to finish:");
            AddListButik(product);
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
        static private string SetPath()
        {
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var inputPath = Path.Combine(userPath, "InputFiles");
            var filePath = Path.Combine(inputPath, "Produkter.json");
            return filePath;
        }
    }
}
