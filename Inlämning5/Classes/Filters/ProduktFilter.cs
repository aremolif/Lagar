using System;
using System.Collections.Generic;
using System.Linq;

namespace Inlämning5.Classes
{
    public class ProduktFilter
    {
        private IProduktRepository ProductRepository { get; }
        private IButikRepository ShopRepository { get; }

        public ProduktFilter(IProduktRepository productRepository, IButikRepository shopRepository)
        {
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
        }
        public ProduktFilter(IProduktRepository productRepository)
        {
            ProductRepository = productRepository;
           
        }
        public ProduktFilter() { }
        
        public ICollection<Butik> SearchProductAvailability(Produkt product)
        {
            if (product.Butik.Count() > 0)
            {
                return product.Butik;
            }
            else
                return new List<Butik>();
        }
        internal void PrintButiker()
        {
            Console.WriteLine($"Product availability:");
            foreach (var butik in SearchProductAvailability(GetProductDetails()))
            {
                Console.WriteLine($"  >{butik.Name}");
            }
        }
        public static Produkt GetProductDetails()
        {
            Console.WriteLine("Insert product name:");
            var newProductName = Console.ReadLine();
            var product = new Produkt() { Name = newProductName };
            Console.WriteLine("Insert product price:");
            product.Price = int.Parse(Console.ReadLine());
            Console.WriteLine("Insert product manufacturer:");
            product.Tillverkare.Name = Console.ReadLine();
            Console.WriteLine("Insert shops - type exit to finish:");


            //product.Butik = Clone(productRepo.AddListButik(product));



            return product;
        }
        public List<Butik> Clone(ICollection<Butik> butikerList)
        {
            // ToList() will give a new List. Otherwise Collection will use the same IList we passed.
            return new List<Butik>(butikerList);
        }
        private ICollection<Butik> AddListButik(Produkt product)
        {
            var butik = new List<Butik>();

            GetButikDetails(product);
            return butik;
        }
        public Butik AddButik(string butikName)
        {
            var butikToAdd = new Butik();
            butikToAdd.Name = butikName;
            return butikToAdd;
        }
        internal ICollection<Butik> GetButikDetails(Produkt product)
        {
            var butik = new List<Butik>();

            var butikToAdd = new Butik();
            while (true)
            {
                var butikName = Console.ReadLine();
                if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (product.Butik.Count > 0))
                    break;
                else
                {
                    if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        butikToAdd = AddButik(butikName);
                        butik.Add(butikToAdd);
                    }
                }
            }
            return butik;


        }

        internal void ChangeProductAvailability(Produkt product)
        {
            PrintShopsWithProduct(product);
            
            Console.WriteLine("Insert shop name");
            string shopDetail = Console.ReadLine();
            
            
            Butik newButik = new Butik() { Name = shopDetail };
            Console.WriteLine("Insert the action to perform:");
            Console.WriteLine("1. To add the product to the shop");
            Console.WriteLine("2. To remove the product from the shop ");
            try
            {
                var shopAction = int.Parse(Console.ReadLine());
                
                var shopChecked = product.Butik.First(s => s.Name == shopDetail);
                
                switch (shopAction)
                {
                    case 1:
                        if (shopChecked == null)
                            product.Butik.Add(newButik);
                        else
                            Console.WriteLine("Shop already registrered");
                        break;
                    case 2:
                        if (shopChecked != null)
                            product.Butik.Remove(shopChecked);
                        else
                            Console.WriteLine("Shop not found");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Try again");
            }
        }

        private void PrintShopsWithProduct(Produkt product)
        {
            Console.WriteLine("Current availability");
            foreach (var butik in product.Butik)
                Console.WriteLine($"  >{butik.Name}"); ;
        }

        public IEnumerable<Produkt> SearchByPrice(decimal maxPrice, IEnumerable<Produkt> products)
        {
            var query = products.Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);
            return query;
        }  
            
        internal void PrintProductFilteredByPrice(decimal maxPrice, IEnumerable<Produkt> FilteredProdukt)
        { 
        if (!FilteredProdukt.Any())
                Console.WriteLine($"No products found with price less than {maxPrice}");
            else
            {
                Console.WriteLine($"\n{"Product",-20}  {"Price",10}");
                foreach (var product in FilteredProdukt)
                    Console.WriteLine($"{product.Name.PadRight(20)}  {product.Price,10}");
                Console.WriteLine("-----");
            }
        }
        internal void GetManufacturersInventory(IEnumerable<Produkt> products)
        {
            var manufactures = products.SelectMany(p => p.Butik, (manufacture, butik) => new
            {
                manufacture,
                butik
            })
                                        .GroupBy(m => m.manufacture.Tillverkare.Name)
                                        .Select(manufacture => new
                                        {
                                            TillverkareName = manufacture.Key,
                                            ProductCount = manufacture.Count()
                                        })
                                         .OrderBy(m => m.TillverkareName);

            Console.WriteLine($"\n{"Manufacturers",-20}  {"Tot. products",10}");
            foreach (var group in manufactures)
                Console.WriteLine("{0,-20} {1, 10}", group.TillverkareName, group.ProductCount);
            Console.WriteLine("-----");
        }
        public IEnumerable<SearchHandler> SearchByLikelihood(string searchString, IEnumerable<Produkt> products)
        {
            var distanceList = new List<SearchHandler>();
            foreach (var p in products)
            {
                var distanceCounter = new SearchHandler();
                distanceCounter.distance = distanceCounter.GetDistance(searchString, p.Name);
                distanceCounter.matchedName = p.Name;
                distanceList.Add(distanceCounter);
            }
            var searchResults = distanceList.Where(d => d.distance > 0.28)
                                            .OrderByDescending(x => x.distance);

            return searchResults;
        }
            

        public void PrintFuzzySearchResults(IEnumerable<SearchHandler> searchResults)
        { 
            Console.WriteLine("Search results:");
            foreach (var s in searchResults)
            Console.WriteLine($">  {s.matchedName}");
            
        }
    }
}
