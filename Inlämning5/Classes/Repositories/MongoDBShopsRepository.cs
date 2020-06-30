using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Inlämning5.Classes.Repositories
{
    public class MongoDbShopsRepository : IShopRepository
    {
        private IMongoCollection<Shop> _collection;
        public MongoDbShopsRepository(IMongoCollection<Shop> collection)
        {
            _collection = collection;
        }
        public IEnumerable<Shop> GetAll()
        {
            var all = _collection.Find(Builders<Shop>.Filter.Empty);
            return all.ToEnumerable();
        }
        public Shop GetById(string id)
        {
            return _collection.Find(Builders<Shop>.Filter.Eq(x => x.Id, id)).FirstOrDefault();
        }
        public void Delete(Shop shop)
        {
            _collection.DeleteOne(s => s.Id == shop.Id);
        }
        public void Insert(Shop shop)
        {
            _collection.InsertOne(shop);
        }
        public void Update(Shop shop)
        {
            _collection.ReplaceOne(p => p.Id == shop.Id, shop);
        }
        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
