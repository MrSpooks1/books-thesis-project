using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace books
{
    public class SelectedProduct : Product
    {
        public int Count { get; set; }
        public SelectedProduct(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            ProductTypeId = product.ProductTypeId;
            SalePrice = product.SalePrice;
            ProviderPrice = product.ProviderPrice;
            OutletProducts = product.OutletProducts;
            ProductType = product.ProductType;
            ReceivedProducts = product.ReceivedProducts;
            SoldProducts = product.SoldProducts;
            Count = 0;
        }
    }
}
