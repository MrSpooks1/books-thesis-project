using System;
using System.Collections.Generic;

namespace books;

public partial class Provider
{
    public int Id { get; set; }

    public int ProviderTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public string? PostalCode { get; set; }

    public virtual ProviderType ProviderType { get; set; } = null!;

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
