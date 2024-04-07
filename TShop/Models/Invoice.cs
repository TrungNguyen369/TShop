using System;
using System.Collections.Generic;

namespace TShop.Models;

public partial class Invoice
{
    public int IdInvoice { get; set; }

    public string IdCustomer { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? Name { get; set; }

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public int StatusCode { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
