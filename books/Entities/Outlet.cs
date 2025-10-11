using System;
using System.Collections.Generic;

namespace books;

public partial class Outlet
{
    public int Id { get; set; }

    public string OutletAddress { get; set; } = null!;

    public virtual ICollection<OutletProduct> OutletProducts { get; set; } = new List<OutletProduct>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
