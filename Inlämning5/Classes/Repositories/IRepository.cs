using System;
using System.Collections.Generic;
using System.Text;

namespace Inlämning5.Classes
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(string id);
        void Delete(T item);
        void Insert(T item);
        void Update(T item);
        void Save();
    }

}
