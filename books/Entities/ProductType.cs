using System;
using System.Collections.Generic;

namespace books;

public partial class ProductType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
