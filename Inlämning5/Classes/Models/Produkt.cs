﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inlämning5.Classes
{
    public class Produkt
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        private  string _name;
        private decimal _price;
               
        public Tillverkare Tillverkare { get; set; }
        public ICollection<Butik> Butik { get; set; }

        public Produkt()  
        {
            this.Tillverkare = new Tillverkare();
            this.Butik = new List<Butik>();
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
        public Butik AddButik(string butikName)
        {
            var butikToAdd = new Butik();
            butikToAdd.Name = butikName;
            return butikToAdd;
        }
        public override string ToString() => $"Name: {Name} Price: {Price} Tillverkare: {Tillverkare.Name} ";
    }
}
