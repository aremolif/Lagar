using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;


namespace Inlämning5.Classes.Repositories
{
    public class MongoDbProduktRepository : IProduktRepository
    {
        
        private IMongoCollection<Produkt> _collection;

        public MongoDbProduktRepository(IMongoCollection<Produkt> collection)
        {
            _collection = collection;
        }

        public IEnumerable<Produkt> GetAll()
        {
            var all = _collection.Find(Builders<Produkt>.Filter.Empty);
            return all.ToEnumerable();
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
            Console.WriteLine($"Id da cercare: {product.Id}");
            _collection.ReplaceOne(p => p.Id == product.Id, product);
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }
    }
}
