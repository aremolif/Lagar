using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Inlämning5.Classes
{
    public class Butik
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
        
        public Butik(string name) 
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
