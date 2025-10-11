using System;
using System.Collections.Generic;

namespace books;

public partial class Shipment
{
    public int Id { get; set; }

    public int ProviderId { get; set; }

    public int EmployeeId { get; set; }

    public int OutletId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Outlet Outlet { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;

    public virtual ICollection<ReceivedProduct> ReceivedProducts { get; set; } = new List<ReceivedProduct>();
}
