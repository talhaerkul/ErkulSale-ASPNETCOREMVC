using System;
using System.Collections.Generic;

namespace erkulSale.entity;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual ICollection<CategoriesProduct> CategoriesProducts { get; set;} = new List<CategoriesProduct>();
}
