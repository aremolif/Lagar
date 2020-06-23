using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace Inlämning5.Classes
{
    public class Butik
    {
        private string _name;
        public string Id { get; set; }
        public Butik()
        {
            this.Produkter = new List<Produkt>();
        }

        public Butik(Butik other)
        {
            _name = other.Name;
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
