using System;
using System.Collections.Generic;

namespace books;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string? Method { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
