using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace Inlämning5.Classes
{
    public class Manufacture
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

                }
                _name = value;
            }
        }
        public override string ToString()
        {
            return Name;
        }

    }
}
