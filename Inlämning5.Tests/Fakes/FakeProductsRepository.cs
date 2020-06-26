using System;
using System.Collections.Generic;
using Inlämning5.Classes;
using MongoDB.Driver;

namespace Inlämning5.Tests
{
    public class FakeProductsRepository : IProduktRepository
    {
        private IEnumerable<Produkt> productsList;

       

        public FakeProductsRepository(IEnumerable<Produkt> products)
        {
            productsList = products;
        }
        

        public IEnumerable<Produkt> GetAll()
        {
            return productsList;
        }

        public Produkt GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Produkt item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Produkt item)
        {
            throw new NotImplementedException();
        }
        public void Delete(Produkt item)
        {
            throw new NotImplementedException();
        }

    }
}
