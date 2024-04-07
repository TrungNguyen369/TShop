using System;
using System.Collections.Generic;

namespace TShop.Models;

public partial class InvoiceDetail
{
    public int IdInvoiceDetail { get; set; }

    public int IdInvoice { get; set; }

    public int IdProduct { get; set; }

    public int IdCustomer { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; } = null!;

    public virtual Invoice IdInvoiceNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
