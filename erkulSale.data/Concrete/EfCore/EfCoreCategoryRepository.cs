using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using erkulSale.data.Abstract;
using erkulSale.entity;

namespace erkulSale.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, SaleContext>, ICategoryRepository
    {
        public void AddCategory(Category category)
        {
            using (var db = new SaleContext())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }

        public void AddProductToCategory(int productId, int categoryId)
        {
            using (var db = new SaleContext())
            {
                var cmd = "insert into categories_products (Products_id,Categories_id) values (@p0,@p1)";
                db.Database.ExecuteSqlRaw(cmd,productId,categoryId);
            }
        }

        public void DeleteCategory(int id)
        {
            using (var db = new SaleContext())
            {
                var result = db.Categories.FirstOrDefault(p => p.Id == id);
                db.Categories.Remove(result);
                db.SaveChanges();
            }
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            using(var context = new SaleContext())
            {
                var cmd = "delete from productcategory where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd,productId,categoryId);
            }
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = null;
            using (var db = new SaleContext())
            {
                categories = db.Categories.ToList();
            }
            return categories;
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            using(var context = new SaleContext())
            {
                return context.Categories
                            .Where(i=>i.Id==categoryId)
                            .Include(i=>i.CategoriesProducts)
                            .ThenInclude(i=>i.Products)
                            .FirstOrDefault();
            }
        }

        public List<Category> GetCategoriesByName(string name)
        {
            List<Category> categories = null;
            using (var db = new SaleContext())
            {
                categories = db.Categories.Where(p => p.Name.Contains(name)).ToList();
            }
            return categories;
        }

        public List<Category> GetCategoriesByProductId(int id)
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

        public Category GetCategoryById(int id)
        {
            using (var db = new SaleContext())
            {
                var category = db.Categories.First(p => p.Id == id);
                return category;
            }
        }

        public Category GetCategoryByUrl(string url)
        {
            using (var db = new SaleContext())
            {
               var category = db.Categories.First(p => p.Url == url);
               return category;
            }
        }

        public Category GetCategoryWithProducts(int categoryId)
        {
            using(var db = new SaleContext())
            {
                var category = db.Categories
                            .Where(i=>i.Id==categoryId)
                            .Include(i=>i.CategoriesProducts)
                            .ThenInclude(i=>i.Products)
                            .First();
                return category;
            }
        }

        public void UpdateCategoryMethod(int id, string name, string url)
        {
            using (var db = new SaleContext())
            {
                var result = db.Categories.First(p => p.Id == id);
                result.Name = name;
                result.Url = url;
                db.Categories.Update(result);
                db.SaveChanges();
            }
        }
    }
}