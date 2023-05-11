using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.entity;

namespace erkulSale.webui.Models
{
    public class ProductDetailModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}