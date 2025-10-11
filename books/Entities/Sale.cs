using System;
using System.Collections.Generic;

namespace books;

public partial class Sale
{
    public int ReceiptNumber { get; set; }

    public int EmployeeId { get; set; }

    public string? CustomerCardNumber { get; set; }

    public DateTime? DateTime { get; set; }

    public int PaymentMethodId { get; set; }

    public int OutletId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Outlet Outlet { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual ICollection<SoldProduct> SoldProducts { get; set; } = new List<SoldProduct>();
}
