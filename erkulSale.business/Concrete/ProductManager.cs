using System.Collections.Generic;
using erkulSale.business.Abstract;
using erkulSale.data.Abstract;
using erkulSale.data.Concrete.EfCore;
using erkulSale.entity;

namespace erkulSale.business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool Create(Product entity)
        {
            if (Validation(entity))
            {
                _productRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Product entity)
        {
            // iş kuralları
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length == 0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz.";
                    return false;
                }
                _productRepository.Update(entity, categoryIds);
                return true;
            }
            return false;
        }

        public string ErrorMessage { get; set; }

        public bool Validation(Product entity)
        {
            var isValid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz.\n";
                isValid = false;
            }

            if (entity.Price < 0)
            {
                ErrorMessage += "ürün fiyatı negatif olamaz.\n";
                isValid = false;
            }

            return isValid;
        }

        public List<Product> GetAllApprovedProducts(int page, int pageSize)
        {
            return _productRepository.GetAllApprovedProducts(page, pageSize);
        }

        public List<Product> GetAllApprovedHomeProducts(int page, int pageSize)
        {
            return _productRepository.GetAllApprovedHomeProducts(page, pageSize);
        }

        public int GetAllApprovedHomeProductsCount()
        {
            return _productRepository.GetAllApprovedHomeProductsCount();
        }

        public List<Product> GetAllProductsByCategoryId(int id, int page, int pageSize)
        {
            return _productRepository.GetAllProductsByCategoryId(id, page, pageSize);
        }

        public void AddProduct(Product entity, int[] categoryIds)
        {
            _productRepository.AddProduct(entity, categoryIds);
        }

        public List<Product> GetAllProducts(int page, int pageSize)
        {
            return _productRepository.GetAllProducts(page, pageSize);
        }

        public Product GetProductWithCategories(int productId)
        {
            return _productRepository.GetProductWithCategories(productId);
        }

        public List<Product> GetAllApprovedProductsByCategoryId(int id)
        {
            return _productRepository.GetAllApprovedProductsByCategoryId(id);
        }

        public List<Product> GetAllApprovedProductsByCategoryUrl(string url, int page, int pageSize)
        {
            return _productRepository.GetAllApprovedProductsByCategoryUrl(url, page, pageSize);
        }

        public int GetCountByCategoryUrl(string? url)
        {
            return _productRepository.GetCountByCategoryUrl(url);

        }

        public int GetCountByCategoryId(int? id)
        {
            return _productRepository.GetCountByCategoryId(id);

        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);

        }

        public Product GetProductByUrl(string url)
        {
            return _productRepository.GetProductByUrl(url);

        }

        public int getCountAllProducts()
        {
            return _productRepository.getCountAllProducts();
        }

        public List<Product> GetProductsByName(string name)
        {
            return _productRepository.GetProductsByName(name);
        }

        public List<Product> GetApprovedProductsByName(string name, int page, int pageSize)
        {
            return _productRepository.GetApprovedProductsByName(name, page, pageSize);

        }

        public void UpdateProductMethod(int id, Product product, int[] categoryIds)
        {
            _productRepository.UpdateProductMethod(id, product, categoryIds);

        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);

        }

        public int pageSize()
        {
            return _productRepository.pageSize();
        }
    }
}