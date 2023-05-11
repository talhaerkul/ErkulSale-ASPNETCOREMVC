using System.Collections.Generic;
using erkulSale.entity;

namespace erkulSale.business.Abstract
{
    public interface IProductService:IValidator<Product>
    {
        int pageSize();
        Product GetById(int id);
        Product GetByIdWithCategories(int id);
        Product GetProductDetails(string url);
        List<Product> GetProductsByCategory(string name,int page,int pageSize);
        int GetCountByCategory(string category);
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string searchString);
        List<Product> GetAll();
        bool Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        bool Update(Product entity, int[] categoryIds);
       List<Product> GetAllApprovedProducts(int page, int pageSize);
       List<Product> GetAllApprovedHomeProducts(int page, int pageSize);
       int GetAllApprovedHomeProductsCount();
       List<Product> GetAllProductsByCategoryId(int id, int page, int pageSize);
       void AddProduct(Product entity, int[] categoryIds);
       List<Product> GetAllProducts(int page, int pageSize);
       Product GetProductWithCategories(int productId);
       List<Product> GetAllApprovedProductsByCategoryId(int id);
       List<Product> GetAllApprovedProductsByCategoryUrl(string url, int page, int pageSize);
       int GetCountByCategoryUrl(string? url);
       int GetCountByCategoryId(int? id);
       Product GetProductById(int id);
       Product GetProductByUrl(string url);
       int getCountAllProducts();
       List<Product> GetProductsByName(string name);
       List<Product> GetApprovedProductsByName(string name, int page, int pageSize);
       void UpdateProductMethod(int id, Product product, int[] categoryIds);
       void DeleteProduct(int id);
    }
}