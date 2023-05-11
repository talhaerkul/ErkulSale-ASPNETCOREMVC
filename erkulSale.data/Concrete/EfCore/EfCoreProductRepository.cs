using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using erkulSale.data.Abstract;
using erkulSale.entity;

namespace erkulSale.data.Concrete.EfCore
{

    public class EfCoreProductRepository :
        EfCoreGenericRepository<Product, SaleContext>, IProductRepository
    {
        public int pageSize()
        {
            int pageSize = 8;
            return pageSize;
        }
        public void AddProduct(Product entity, int[] categoryIds)
        {
            using (var db = new SaleContext())
            {
                db.Products.Add(entity);
                db.SaveChanges();
            }
            using (var db2 = new SaleContext())
            {
                var product = db2.Products
                                    .Include(i => i.CategoriesProducts)
                                    .FirstOrDefault(i => i.Id == entity.Id);
                product.CategoriesProducts = categoryIds.Select(catid => new CategoriesProduct()
                {
                    ProductsId = entity.Id,
                    CategoriesId = catid
                }).ToList();
                // if (categoryIds.Count() != 0)
                // {
                //     foreach (var item in categoryIds)
                //     {
                //         CategoryMethods.AddProductToCategory(product.Id, item);
                //     }
                // }
                db2.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            using (var db = new SaleContext())
            {
                var result = db.Products.FirstOrDefault(p => p.Id == id);
                if (result is not null)
                {
                    db.Products.Remove(result);
                    db.SaveChanges();
                }
            }
        }

        public List<Product> GetAllApprovedHomeProducts(int page, int pageSize)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.Where(p => p.IsApproved == 1 && p.IsHome == 1).ToList();
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        }

        public int GetAllApprovedHomeProductsCount()
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.Where(p => p.IsApproved == 1 && p.IsHome == 1).ToList();
            }
            return products.Count();
        }

        public List<Product> GetAllApprovedProducts(int page, int pageSize)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.Where(p => p.IsApproved == 1).ToList();
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> GetAllApprovedProductsByCategoryId(int id)
        {
            using (var db = new SaleContext())
            {
                var products = db.Products.AsQueryable();
                products = products.Include(i => i.CategoriesProducts)
                    .ThenInclude(i => i.Categories)
                    .Where(c => c.CategoriesProducts.Any(a => a.CategoriesId == id));
                products = products.Where(p => p.IsApproved == 1);
                return products.ToList();
            }
        }

        public List<Product> GetAllApprovedProductsByCategoryUrl(string url, int page, int pageSize)
        {
            using (var db = new SaleContext())
            {
                var products = db.Products.AsQueryable();
                if (!string.IsNullOrEmpty(url))
                {
                    products = products.Include(i => i.CategoriesProducts)
                        .ThenInclude(i => i.Categories)
                        .Where(i => i.CategoriesProducts.Any(a => a.Categories.Url.Trim().ToLower() == url.Trim().ToLower()));
                }
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Product> GetAllProducts(int page, int pageSize)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.ToList();
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Product> GetAllProductsByCategoryId(int id, int page, int pageSize)
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

        public List<Product> GetApprovedProductsByName(string name, int page, int pageSize)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.Where(p => p.IsApproved == 1 && (p.Name.Trim().ToLower().Contains(name.Trim().ToLower()) || p.Description.ToLower().Contains(name.ToLower()))).ToList();
            }
            return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public Product GetByIdWithCategories(int id)
        {
            using (var context = new SaleContext())
            {
                return context.Products
                                .Where(i => i.Id == id)
                                .Include(i => i.CategoriesProducts)
                                .ThenInclude(i => i.Categories)
                                .FirstOrDefault();
            }
        }

        public int getCountAllProducts()
        {
            using (var db = new SaleContext())
            {
                return db.Products.Count();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new SaleContext())
            {
                var products = context.Products.Where(i => i.IsApproved == 1).AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                    .Include(i => i.CategoriesProducts)
                                    .ThenInclude(i => i.Categories)
                                    .Where(i => i.CategoriesProducts.Any(a => a.Categories.Url == category));
                }

                return products.Count();
            }
        }

        public int GetCountByCategoryId(int? id)
        {
            using (var db = new SaleContext())
            {
                var products = db.Products
                .Include(p => p.CategoriesProducts)
                .ThenInclude(cp => cp.Categories)
                .Where(p => p.CategoriesProducts.Any(cp => cp.CategoriesId == id) && p.IsApproved == 1);
                return products.Count();
            }
        }

        public int GetCountByCategoryUrl(string? url)
        {
            using (var db = new SaleContext())
            {
                var products = db.Products.AsQueryable();
                if (!string.IsNullOrEmpty(url))
                {
                    products = products.Include(i => i.CategoriesProducts)
                        .ThenInclude(i => i.Categories)
                        .Where(i => i.CategoriesProducts.Any(a => a.Categories.Url.Trim().ToLower() == url.Trim().ToLower()));
                }
                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var context = new SaleContext())
            {
                return context.Products
                    .Where(i => i.IsApproved == 1 && i.IsHome == 1).ToList();
            }
        }

        public Product GetProductById(int id)
        {
            using (var db = new SaleContext())
            {
                return db.Products.FirstOrDefault(p => p.Id == id);
            }
        }

        public Product GetProductByUrl(string url)
        {
            using (var db = new SaleContext())
            {
                return db.Products.First(p => p.Url == url);
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new SaleContext())
            {
                return context.Products
                                .Where(i => i.Url == url)
                                .Include(i => i.CategoriesProducts)
                                .ThenInclude(i => i.Categories)
                                .FirstOrDefault();

            }
        }
        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using (var context = new SaleContext())
            {
                var products = context
                    .Products
                    .Where(i => i.IsApproved == 1)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                                    .Include(i => i.CategoriesProducts)
                                    .ThenInclude(i => i.Categories)
                                    .Where(i => i.CategoriesProducts.Any(a => a.Categories.Url == name));
                }

                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Product> GetProductsByName(string name)
        {
            List<Product> products = null;
            using (var db = new SaleContext())
            {
                products = db.Products.Where(p => p.Name.Trim().ToLower().Contains(name.Trim().ToLower()) || p.Description.ToLower().Contains(name.ToLower())).ToList();
            }
            return products;
        }

        public Product GetProductWithCategories(int productId)
        {
            using (var db = new SaleContext())
            {
                var product = db.Products
                            .Where(i => i.Id == productId)
                            .Include(i => i.CategoriesProducts)
                            .ThenInclude(i => i.Categories)
                            .First();
                return product;
            }
        }

        public List<Product> GetSearchResult(string searchString)
        {
            using (var context = new SaleContext())
            {
                var products = context
                    .Products
                    .Where(i => i.IsApproved == 1 && (i.Name.ToLower().Contains(searchString.ToLower()) || i.Description.ToLower().Contains(searchString.ToLower())))
                    .AsQueryable();

                return products.ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new SaleContext())
            {
                var product = context.Products
                                    .Include(i => i.CategoriesProducts)
                                    .FirstOrDefault(i => i.Id == entity.Id);


                if (product != null)
                {
                    product.Name = entity.Name;
                    product.Price = entity.Price;
                    product.Description = entity.Description;
                    product.Url = entity.Url;
                    product.ImgUrl = entity.ImgUrl;
                    product.IsApproved = entity.IsApproved;
                    product.IsHome = entity.IsHome;

                    product.CategoriesProducts = categoryIds.Select(catid => new CategoriesProduct()
                    {
                        ProductsId = entity.Id,
                        CategoriesId = catid
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }

        public void UpdateProductMethod(int id, Product product, int[] categoryIds)
        {
            using (var db = new SaleContext())
            {
                var result = db.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    result.Name = product.Name;
                    result.Url = product.Url;
                    result.Price = product.Price;
                    result.Description = product.Description;
                    result.ImgUrl = product.ImgUrl;
                    result.IsApproved = product.IsApproved;
                    result.IsHome = product.IsHome;

                    product.CategoriesProducts = categoryIds.Select(catid => new CategoriesProduct()
                    {
                        ProductsId = result.Id,
                        CategoriesId = catid
                    }).ToList();

                    db.Products.Update(result);
                    db.SaveChanges();
                }
            }
        }
    }
}