using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;


namespace Inlämning5.Classes.Repositories
{
    public class MongoDbProductsRepository : IProduktRepository
    {
        
        private IMongoCollection<Product> _collection;
        public MongoDbProductsRepository(IMongoCollection<Product> collection)
        {
            
            _collection = collection;
        }
        public IEnumerable<Product> GetAll()
        {
            var all = _collection.Find(Builders<Product>.Filter.Empty);
            return all.ToEnumerable();
        }
        public Product GetById(string id)
        {
            return _collection.Find(Builders<Product>.Filter.Eq(x => x.Id, id)).FirstOrDefault();
        }
        public void Delete(Product product)
        {
            //Builders<Product>.Filter.Eq(x => x.Id, id)
            _collection.DeleteOne(s => s.Id == product.Id);
        }
        public void Insert(Product product)
        {
            _collection.InsertOne(product);
        }
        public void Update(Product product)
        {
            _collection.ReplaceOne(p => p.Id == product.Id, product);
        }
        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
