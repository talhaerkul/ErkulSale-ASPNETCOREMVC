using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using erkulSale.data.Concrete.EfCore;
using erkulSale.entity;
using Microsoft.EntityFrameworkCore;

namespace erkulSale.webui.Service
{
    public class AdminViewUpdateCategoryService
    {
        public static List<Product> GetAllProducts(int page, int pageSize)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.ToList();
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
        public static List<Product> GetAllProductsByCategoryId(int id, int page, int pageSize)
        {
            using (var db = new SaleContext())
            {
                var products = db.Products.AsQueryable();
                products = products.Include(i => i.CategoriesProducts)
                    .ThenInclude(i => i.Categories)
                    .Where(c => c.CategoriesProducts.Any(a => a.CategoriesId == id));
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}