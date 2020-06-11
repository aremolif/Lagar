using System;
using System.Collections.Generic;
using System.Text;

namespace Inlämning4.Classes
{
    public interface IProduktRepository
    {
        IEnumerable<Produkt> GetAll();
        Produkt GetByProductName(string ProductName);
        void Delete(Produkt produkt);
        void Add(Produkt produkt);
        void Save();
    }
}
