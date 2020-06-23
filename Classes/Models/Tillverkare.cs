using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace Inlämning5.Classes
{
    public class Tillverkare
    {
        private string _name;
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
        public Tillverkare()
        {
            this.Produkter = new List<Produkt>();
        }
        [JsonIgnore]
        public ICollection<Produkt> Produkter { get; set; }
    }
}
