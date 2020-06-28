using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes.Filters;

namespace Inlämning5.Classes
{
    public class ProductFilters
    {
        private IProduktRepository ProductRepository { get; }
        private IButikRepository ShopRepository { get; }
        public ProductFilters(IProduktRepository productRepository, IButikRepository shopRepository)
        {
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
        }
        public IEnumerable<Product> SearchByPrice(decimal maxPrice)
        {
            return ProductRepository.GetAll().Where(s => s.Price < maxPrice).OrderByDescending(p => p.Price).Take(10);

        }
        public IEnumerable<Product> SearchProductByName(string name)
        {
            return ProductRepository.GetAll().Where(p => p.Name.Equals(name));

        }
        public IEnumerable<Shop> SearchShopByName(string name)
        {
            return ShopRepository.GetAll().Where(p => p.Name.Equals(name));
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
        public IEnumerable<Product> GetAllStock()
        {
            return ProductRepository.GetAll();
        }
        
        public void GetManufacturersInventory()
        {

            var products = ProductRepository.GetAll();


            var manufactures = products.SelectMany(p => p.Shops, (manufacturer, shop) => new
            {
                manufacturer,
                shop
            })
                                        .GroupBy(m => m.manufacturer.Manufacturer.Name)
                                        .Select(manufacturer => new
                                        {
                                            ManufacturerName = manufacturer.Key,
                                            ProductCount = manufacturer.Count()
                                        })
                                         .OrderBy(m => m.ManufacturerName);

            Console.WriteLine($"\n{"Manufacturers",-20}  {"Tot. products",10}");
            foreach (var group in manufactures)
                Console.WriteLine("{0,-20} {1, 10}", group.ManufacturerName, group.ProductCount);
            Console.WriteLine("-----");
        }
        public void AddProductToCollection(Product product)
        {
            ProductRepository.Insert(product);

        }
        public Shop AddShopToCollection(string shopName)
        {
            var newShop = new Shop(shopName);
            ShopRepository.Insert(newShop);
            newShop.Id = SearchShopByName(shopName).First().Id;
            return newShop;

        }
        public void UpdateProductPrice(Product product)
        {
            var matches = SearchProductByName(product.Name);
            if (matches.Any())
            {
                product.Id = matches.First().Id;
                product.Name = matches.First().Name;
                product.Manufacturer = matches.First().Manufacturer;
                product.Shops = matches.First().Shops;
                ProductRepository.Update(product);
            }
            else
                throw new InvalidOperationException();

        }
        public void UpdateProductName(Product product)
        {
            var matches = SearchProductByName(product.Name);
            if (matches.Any())
            {
                product.Id = matches.First().Id;
                product.Price = matches.First().Price;
                product.Manufacturer = matches.First().Manufacturer;
                product.Shops = matches.First().Shops;
                ProductRepository.Update(product);
            }
            else
                throw new InvalidOperationException();

        }
        public void UpdateExistingProductInCollection(Product product)
        {
            ProductRepository.Update(product);
            
        }
        public void UpdateProductAvailability(Product newProduct, string butikName)
        {
            var newShop = new Shop();
            if (!SearchShopByName(butikName).Any())
            {
                newShop = AddShopToCollection(butikName);
            }
            newProduct.AddShop(newShop);
        }
        public void RemoveProductFromCollection(string productName)
        {
            var matches = SearchProductByName(productName);
            ProductRepository.Delete(matches.First());


        }
        public void RemoveShopFromCollection(string shopName)
        {
            var shopToRemove = SearchShopByName(shopName);
            if (shopToRemove.Any())
            {
                ShopRepository.Delete(shopToRemove.First());
            }
            var productsList = ProductRepository.GetAll().ToList();
            foreach (var product in productsList)
            {

                var matches = product.Shops.Where(b => b.Name == shopName);
                if (matches.Any())
                    product.RemoveShop(matches.First());
            }
        }
        

    }
}
