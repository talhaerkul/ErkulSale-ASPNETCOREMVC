using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.entity;


namespace erkulSale.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Display(Name="Kategori Adı")]
        public string Name { get; set; }
        
        [Display(Name="Kategori Url")]

        public string Url { get; set; }

        public List<Product> Products { get; set; }
    }
}