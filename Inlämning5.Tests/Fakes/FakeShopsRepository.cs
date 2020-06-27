using System;
using System.Collections.Generic;
using System.Text;
using Inlämning5.Classes;

namespace Inlämning5.Tests
{
    public class FakeShopsRepository : IButikRepository
    {
        private List<Butik> shopsList;
        public FakeShopsRepository(List<Butik> shops)
        {
            shopsList = shops;
        }

        public void Delete(Butik item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Butik> GetAll()
        {
            return shopsList;
        }

        public Butik GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Butik shop)
        {
            shopsList.Add(shop); ;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Butik item)
        {
            throw new NotImplementedException();
        }
    }
}
