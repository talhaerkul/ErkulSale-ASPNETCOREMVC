using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.data.Concrete.EfCore;
using erkulSale.entity;
using Microsoft.EntityFrameworkCore;

namespace erkulSale.webui.Service
{
    public class ProductDetailService
    {
        public static List<Category> GetCategoriesByProductId(int id)
        {
            using (var db = new SaleContext())
            {
                var categories = db.Categories.AsQueryable();
                categories = categories.Include(i => i.CategoriesProducts)
                    .ThenInclude(i => i.Products)
                    .Where(i => i.CategoriesProducts.Any(a => a.ProductsId == id));
                return categories.ToList();
            }
        }
        public static List<Product> GetProductsByName(string name)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.Where(p => p.Name.Trim().ToLower().Contains(name.Trim().ToLower()) || p.Description.ToLower().Contains(name.ToLower())).ToList();
            }
            return products;
        }
    }
}