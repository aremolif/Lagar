using System;
using System.Collections.Generic;
using System.Text;

namespace Inlämning5.Classes.Models
{
    public class TillverkareService
    {
        private static List<Manufacture> _manufacturers = new List<Manufacture>()
        {
            new Manufacture() {Name = "LG"}, 
            new Manufacture() {Name = "Philips"},
            new Manufacture() {Name = "Dell"}, 
            new Manufacture() {Name = "Sony"},
            new Manufacture() {Name = "Electrolux" },
            new Manufacture() {Name = "Huawei"},
            new Manufacture() {Name = "Dyson"},
            new Manufacture() {Name = "Wilfa"},
            new Manufacture() {Name = "DeLonghi"},
            new Manufacture() {Name = "Beurer"},
            new Manufacture() {Name = "Tristar"},
            new Manufacture() {Name = "Russell Hobbs"},
            new Manufacture() {Name = "GoPro"},
            new Manufacture() {Name = "Lenovo"}
        };

        public static Manufacture GetManufacturer(int id)
        {
            return _manufacturers[id];
        }
    }
}
