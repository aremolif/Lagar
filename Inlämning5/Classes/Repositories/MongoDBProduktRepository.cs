using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;


namespace Inlämning5.Classes.Repositories
{
    public class MongoDbProduktRepository : IProduktRepository
    {
        //private IMongoCollection<Produkt> _collection;
        private MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Produkt> _collection;

        public MongoDbProduktRepository(IMongoCollection<Produkt> collection)
        {
            _collection = collection;
        }

        public MongoDbProduktRepository()
        {
            _client = new MongoClient("mongodb://localhost:27020");
            _database = _client.GetDatabase("Warehouse");
            _collection = _database.GetCollection<Produkt>("Products");
            //if (!_database.Ping())
            //    throw new Exception("Could not connect to MongoDb");
        }

        
        
        public IEnumerable<Produkt> GetAll()
        {
            var all = _collection.Find(Builders<Produkt>.Filter.Empty).ToEnumerable();

            //return all.ToEnumerable();
            return all;
        }

        public Produkt GetById(string id)
        {
            return _collection.Find(Builders<Produkt>.Filter.Eq(x => x.Id, id)).FirstOrDefault();
        }

        public void Delete(Produkt product)
        {
            //Builders<Product>.Filter.Eq(x => x.Id, id)
            _collection.DeleteOne(s => s.Id == product.Id);
        }

        public void Insert(Produkt product)
        {
            _collection.InsertOne(product);
        }

        public void Update(Produkt product)
        {
            _collection.ReplaceOne(p => p.Id == product.Id, product);
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
