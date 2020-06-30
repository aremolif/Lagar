using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inlämning5.Classes.Helpers
{
    public class EntitiesHelper
    {
        private IProductRepository ProductRepository { get; }
        private IShopRepository ShopRepository { get; }
        public EntitiesHelper(IProductRepository productRepository, IShopRepository shopRepository)
        {
            ProductRepository = productRepository;
            ShopRepository = shopRepository;
        }
        public IEnumerable<Product> GetAllStock()
        {
            return ProductRepository.GetAll();
        }
        public IEnumerable<Product> GetProductByName(string name)
        {
            return ProductRepository.GetAll().Where(p => p.Name.Equals(name));
        }
        public IEnumerable<Shop> GetShopByName(string name)
        {
            return ShopRepository.GetAll().Where(p => p.Name.Equals(name));
        }
        public void AddProductToCollection(Product product)
        {
            ProductRepository.Insert(product);
        }
        public void RemoveProductFromCollection(string productName)
        {
            var matches = GetProductByName(productName);
            ProductRepository.Delete(matches.First());
        }
        public void UpdateProductInCollection(Product product)
        {
            ProductRepository.Update(product);
        }
        public Shop AddShopToCollection(string shopName)
        {
            var newShop = new Shop(shopName);
            ShopRepository.Insert(newShop);
            newShop.Id = GetShopByName(shopName).First().Id;
            return newShop;
        }
        public void RemoveShopFromCollection(string shopName)
        {
            var shopToRemove = GetShopByName(shopName);
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
        public Product UpdateShopsWithinProduct(Product newProduct, string shopName)
        {
            var newShop = new Shop() { Name = shopName};
            if (!GetShopByName(shopName).Any())
                newShop = AddShopToCollection(shopName);
            newShop.Id = GetShopByName(shopName).First().Id;
            newProduct.AddShop(newShop);
            return newProduct;
        }
        public void UpdateProductPrice(Product product)
        {
            var matches = GetProductByName(product.Name);
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
        public void UpdateProductName(string newName, Product product)
        {
            var matches = GetProductByName(product.Name);
            if (matches.Any())
            {
                product.Name = newName;
                product.Id = matches.First().Id;
                product.Price = matches.First().Price;
                product.Manufacturer = matches.First().Manufacturer;
                product.Shops = matches.First().Shops;
                ProductRepository.Update(product);
            }
            else
                throw new InvalidOperationException();

        }
    }
}
