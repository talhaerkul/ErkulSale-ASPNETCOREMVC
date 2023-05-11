using System;
using System.Collections.Generic;

namespace erkulSale.entity;

public partial class CategoriesProduct
{
    public int Id { get; set; }

    public int CategoriesId { get; set; }

    public int ProductsId { get; set; }

    public virtual Category Categories { get; set; } = null!;

    public virtual Product Products { get; set; } = null!;
}
