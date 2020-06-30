using System;
using System.Collections.Generic;
using System.Linq;
using Inlämning5.Classes;
using MongoDB.Driver;

namespace Inlämning5.Tests
{
    public class FakeProductsRepository : IProductRepository
    {
        private IEnumerable<Product> productsList;
        public FakeProductsRepository(IEnumerable<Product> products)
        {
            productsList = products;
        }
        public IEnumerable<Product> GetAll()
        {
            return productsList;
        }
        public Product GetById(string id)
        {
            throw new NotImplementedException();
        }
        public void Insert(Product item)
        {
            throw new NotImplementedException();
        }
        public void Save()
        {
            throw new NotImplementedException();
        }
        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
        public void Delete(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
