using System;
using System.Collections.Generic;

namespace erkulSale.entity;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public sbyte? IsApproved { get; set; }

    public string? ImgUrl { get; set; }

    public string Url { get; set; } = null!;
    
    public sbyte? IsHome { get; set; }

    public virtual ICollection<CategoriesProduct> CategoriesProducts { get; set;} = new List<CategoriesProduct>();
}
