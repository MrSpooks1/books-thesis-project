using System;
using System.Collections.Generic;

namespace books;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProductTypeId { get; set; }

    public decimal SalePrice { get; set; }

    public decimal ProviderPrice { get; set; }

    public virtual ICollection<OutletProduct> OutletProducts { get; set; } = new List<OutletProduct>();

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<ReceivedProduct> ReceivedProducts { get; set; } = new List<ReceivedProduct>();

    public virtual ICollection<SoldProduct> SoldProducts { get; set; } = new List<SoldProduct>();
}
