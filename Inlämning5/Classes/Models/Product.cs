using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inlämning5.Classes
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        private  string _name;
        private decimal _price;
        public Manufacturer Manufacturer { get; set; }
        public ICollection<Shop> Shops { get; set; }
        public Product()  
        {
            this.Manufacturer = new Manufacturer();
            this.Shops = new List<Shop>();
        }
        public string Name
        {
            get
            { return _name; }

            set
            {
                if (string.IsNullOrEmpty(value))
                {

                    throw new ArgumentNullException(nameof(Name));
                }
                _name = value;
            }
        }
        public decimal Price
        {
            get { return _price; }
            set { if (value < 0)
                {

                    throw new ArgumentNullException(nameof(Name));
                }
                _price = value; } }
        public void AddShop(Shop shop)
        {
            Shops.Add(shop);   
        }
        public void RemoveShop(Shop shop)
        {
            Shops.Remove(shop);
        }
        public override string ToString() => $"Name: {Name} Price: {Price} Tillverkare: {Manufacturer.Name} ";
    }
}
