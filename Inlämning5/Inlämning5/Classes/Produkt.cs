using System;
using System.Collections.Generic;

namespace Inlämning4.Classes
{
    public class Produkt
    {
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
        
        public override string ToString() => $"Name: {Name} Price: {Price} Tillverkare: {Tillverkare.Name} ";
    }
}
