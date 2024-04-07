using System;
using System.Collections.Generic;

namespace TShop.Models;

public partial class Customer
{
    public int IdCustomer { get; set; }

    public string? PassWord { get; set; }

    public string FullName { get; set; } = null!;

    public bool Sex { get; set; }

    public DateTime BirthDay { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public bool Effect { get; set; }

    public string? RandomKey { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
