using System;
using System.Collections.Generic;
using System.Text;
using Inlämning5.Classes;

namespace Inlämning5.Tests
{
    public class FakeShopsRepository : IShopRepository
    {
        private List<Shop> shopsList;
        public FakeShopsRepository(List<Shop> shops)
        {
            shopsList = shops;
        }
        public void Delete(Shop item)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Shop> GetAll()
        {
            return shopsList;
        }
        public Shop GetById(string id)
        {
            throw new NotImplementedException();
        }
        public void Insert(Shop shop)
        {
            shopsList.Add(shop); ;
        }
        public void Save()
        {
            throw new NotImplementedException();
        }
        public void Update(Shop item)
        {
            throw new NotImplementedException();
        }
    }
}
