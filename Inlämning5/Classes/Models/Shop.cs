using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inlämning5.Classes
{
    public class Shop
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        private string _name;
        public string Name { 
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
        public Shop(string name) 
        {
            Name = name;
        }
        public Shop() { }
        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
