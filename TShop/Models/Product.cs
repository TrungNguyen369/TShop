using System;
using System.Collections.Generic;

namespace TShop.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public int IdBrands { get; set; }

    public int IdCategory { get; set; }

    public string NameProduct { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? Image { get; set; }

    public string? ImageDetailOne { get; set; }

    public string? ImageDetailTwo { get; set; }

    public virtual Brand IdBrandsNavigation { get; set; } = null!;

    public virtual Category IdCategoryNavigation { get; set; } = null!;

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
