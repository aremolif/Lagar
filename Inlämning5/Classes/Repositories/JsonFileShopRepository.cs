using System;
using System.Collections.Generic;
using System.Linq;

namespace Inlämning5.Classes
{
    class JsonFileShopRepository : IButikRepository
    {

        public JsonFileHandler JsonFileHandler { get; }
        //public List<Produkt> ProductsList { get; set; }
        //private readonly List<Butik> _shops;
        private readonly List<Produkt> _produkts;
        public JsonFileShopRepository(JsonFileHandler handler)
        {
            JsonFileHandler = handler;
            _produkts = handler.Retrieve().ToList();
           // _shops = 
        }
        public void Delete(Butik item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Butik> GetAll()
        {
            throw new NotImplementedException();
        }

        public Butik GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Butik item)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Butik item)
        {
            throw new NotImplementedException();
        }
    }
}
