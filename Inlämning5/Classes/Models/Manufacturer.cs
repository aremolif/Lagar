using System;

namespace Inlämning5.Classes
{
    public class Manufacturer
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
