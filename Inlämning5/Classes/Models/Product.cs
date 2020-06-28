using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inlämning5.Classes
{
    public class Product
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        private  string _name;
        private decimal _price;
               
        public Manufacture Tillverkare { get; set; }
        public ICollection<Shop> Butik { get; set; }

        public Product()  
        {
            this.Tillverkare = new Manufacture();
            this.Butik = new List<Shop>();
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
            Butik.Add(shop);   
        }
        public void RemoveShop(Shop shop)
        {
            Butik.Remove(shop);
        }

        public override string ToString() => $"Name: {Name} Price: {Price} Tillverkare: {Tillverkare.Name} ";
    }
}
