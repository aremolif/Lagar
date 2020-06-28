using System;
using System.Collections.Generic;
using System.Text;

namespace Inlämning5.Classes.Filters
{
    public interface IProduktFilter
    {
        void AddNewProduct();
        Shop UpdateShopCollection(string shopName);
        void UpdateProductName();
        void RemoveProduct();
        void SearchProductByPrice();
        void SearchProduct();
        void ListShopFromStock();
        void RemoveShopFromStock(); //toglie dai prodotti e dal negozio
        void UpdateProductPrice();
        void AddShopAvailability();
        void RemoveShopAvailability();
        
    }
}
