using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes.Filters;

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

        public Butik UpdateShopCollection(string shopName)
        {
            var newShop = new Butik();
            ShopRepository.Insert(newShop);
            newShop.Id = SearchShopByName(shopName).First().Id;
            return newShop;
            
        }
        public IEnumerable<Produkt> SearchByPrice(decimal maxPrice)
        {
            return ProductRepository.GetAll().Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);
            
        }
        public IEnumerable<Produkt> SearchProductByName(string name)
        {
            return ProductRepository.GetAll().Where(p=> p.Name.Equals(name));
            
        }
        public IEnumerable<Butik> SearchShopByName(string name)
        {
            return ShopRepository.GetAll().Where(p => p.Name.Equals(name)); 
        }
        public IEnumerable<Produkt> SearchAllStock()
        {
            return ProductRepository.GetAll();
        }
        public IEnumerable<Butik> GetShopsWithProduct(Produkt product)
        {
            return product.Butik;
        }
        public void GetManufacturersInventory()
        {

            var products = ProductRepository.GetAll();
            

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
        public void RemoveShopFromStock(string shopName)
        {
            var shopToRemove = SearchShopByName(shopName);
            if (shopToRemove.Any())
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
        public IEnumerable<SearchHandler> SearchByLikelihood(string searchString)
        {
            var products = ProductRepository.GetAll();
            var distanceList = new List<SearchHandler>();
            foreach (var p in products)
            {
                var distanceCounter = new SearchHandler();
                distanceCounter.Distance = distanceCounter.GetDistance(searchString, p.Name);
                distanceCounter.MatchedName = p.Name;
                distanceList.Add(distanceCounter);
            }
            var searchResults = distanceList.Where(d => d.Distance > 0.28)
                                            .OrderByDescending(x => x.Distance);

            return searchResults;
        }
        public void UpdateCollections(Produkt newProduct, string butikName)
        {
            var newShop = new Butik();
            if (!SearchShopByName(butikName).Any())
            {
                newShop = UpdateShopCollection(butikName);
            }
            newProduct.AddShop(newShop);
        }

    }
}
