using System;
using System.Collections.Generic;

namespace TShop.Models;

public partial class Brand
{
    public int IdBrands { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageBrands { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
