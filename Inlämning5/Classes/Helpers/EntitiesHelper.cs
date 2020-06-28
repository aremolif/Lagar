using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inlämning5.Classes.Helpers
{
    public class EntitiesHelper
    {
        private IProduktRepository ProductRepository { get; }
        private IButikRepository ShopRepository { get; }
        public EntitiesHelper(IProduktRepository productRepository, IButikRepository shopRepository)
        {
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
        }

        public IEnumerable<Product> GetAllStock()
        {
            return ProductRepository.GetAll();
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
        public void AddShopsToProduct(Product newProduct)
        {
            while (true)
            {
                var butikName = Console.ReadLine();
                if (butikName.Equals("exit", StringComparison.OrdinalIgnoreCase) && (newProduct.Shops.Count > 0))
                    break;
                else
                {
                    if (!butikName.Equals("exit", StringComparison.OrdinalIgnoreCase))
                        UpdateShopsWithinProduct(newProduct, butikName);

                }
            }
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
        public void UpdateShopsWithinProduct(Product newProduct, string butikName)
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
        public IEnumerable<Product> SearchProductByName(string name)
        {
            return ProductRepository.GetAll().Where(p => p.Name.Equals(name));

        }
        public IEnumerable<Shop> SearchShopByName(string name)
        {
            return ShopRepository.GetAll().Where(p => p.Name.Equals(name));
        }
    }
}
