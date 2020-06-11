using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Inlämning4.Classes
{
    public class JsonFileRepository : IProduktRepository
    {
        public JsonFileHandler JsonFileHandler { get; }   
        public List<Produkt> ProductsList { get; set; }      
        public JsonFileRepository(JsonFileHandler handler)
        {
            JsonFileHandler = handler;
            ProductsList = handler.Retrieve().ToList();
        }
        public IEnumerable<Produkt> GetAll()
        {
            return ProductsList;
        }
        public Produkt GetByProductName(string produktName)
        {
            Produkt product = new Produkt();
            return product = ProductsList.Find(x => x.Name == produktName);
        }
        public void Add(Produkt newProdukt)
        {
            ProductsList.Add(newProdukt);
        }
        public void Delete(Produkt produkt)
        {
            int index = ProductsList.FindIndex(p => p.Name == produkt.Name);
            if (index >= 0)
            {   
                ProductsList.RemoveAt(index);
                JsonFileHandler.SaveChanges(ProductsList);
            }
            else
                Console.WriteLine("Product not found");
        }
        public void Save()
        {
            JsonFileHandler.SaveChanges(ProductsList); 
        }
    }
}



