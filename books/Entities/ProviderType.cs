using System;
using System.Collections.Generic;

namespace books;

public partial class ProviderType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Provider> Providers { get; set; } = new List<Provider>();
}
