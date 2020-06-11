using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace Inlämning4.Classes
{
    public class Butik
    {
        private string _name; 
        public Butik()
        {
            this.Produkter = new List<Produkt>();
        }
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
        
        [JsonIgnore]
        public ICollection<Produkt> Produkter { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}
