using System;
using System.Collections.Generic;

namespace books;

public partial class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string PassportSerialNumber { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string Login { get; set; } = null!;
    public int AccessLevel { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
