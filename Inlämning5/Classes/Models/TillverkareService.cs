using System;
using System.Collections.Generic;
using System.Text;

namespace Inlämning5.Classes.Models
{
    public class TillverkareService
    {
        private static List<Tillverkare> _manufacturers = new List<Tillverkare>()
        {
            new Tillverkare() {Name = "LG"}, 
            new Tillverkare() {Name = "Philips"},
            new Tillverkare() {Name = "Dell"}, 
            new Tillverkare() {Name = "Sony"},
            new Tillverkare() {Name = "Electrolux" },
            new Tillverkare() {Name = "Huawei"},
            new Tillverkare() {Name = "Dyson"},
            new Tillverkare() {Name = "Wilfa"},
            new Tillverkare() {Name = "DeLonghi"},
            new Tillverkare() {Name = "Beurer"},
            new Tillverkare() {Name = "Tristar"},
            new Tillverkare() {Name = "Russell Hobbs"},
            new Tillverkare() {Name = "GoPro"},
            new Tillverkare() {Name = "Lenovo"}
        };

        public static Tillverkare GetManufacturer(int id)
        {
            return _manufacturers[id];
        }
    }
}
