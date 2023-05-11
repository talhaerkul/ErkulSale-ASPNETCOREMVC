using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.entity;

namespace erkulSale.webui.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        
        [Display(Name="Ürün Adı",Prompt ="Ürün Adını Giriniz")]
        public string Name { get; set; } = null!;

        [Display(Name="Açıklama",Prompt ="Açıklama Giriniz")]

        public string? Description { get; set; }

        [Display(Name="Fiyat",Prompt ="Fiyat Giriniz")]
        public decimal Price { get; set; }

        [Display(Name="Onaylı Ürün")]
        public sbyte? IsApproved { get; set; }

        [Display(Name="Ürün Görseli")]
        public string? ImgUrl { get; set; }

        public string Url { get; set; } = null!;

        [Display(Name="Ana Sayfada Göster")]

        public sbyte? IsHome { get; set; }
        
        public List<Category> SelectedCategories { get; set; }
    }
}