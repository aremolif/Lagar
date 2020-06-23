using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Inlämning5.Classes.Repositories
{
    public class MongoDbButikRepository : IButikRepository
    {
        private IMongoCollection<Butik> _collection;

        public MongoDbButikRepository(IMongoCollection<Butik> collection)
        {
            _collection = collection;
        }

        public IEnumerable<Butik> GetAll()
        {
            var all = _collection.Find(Builders<Butik>.Filter.Empty);
            return all.ToEnumerable();
        }

        public Butik GetById(string id)
        {
            return _collection.Find(Builders<Butik>.Filter.Eq(x => x.Id, id)).FirstOrDefault();
        }

        public void Delete(Butik shop)
        {
            //Builders<Product>.Filter.Eq(x => x.Id, id)
            _collection.DeleteOne(s => s.Id == shop.Id);
        }

        public void Insert(Butik shop)
        {
            _collection.InsertOne(shop);
        }

        public void Update(Butik shop)
        {
            _collection.ReplaceOne(p => p.Id == shop.Id, shop);
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

    }
}
