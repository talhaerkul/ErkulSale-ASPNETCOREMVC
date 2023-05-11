using System.Collections.Generic;
using erkulSale.business.Abstract;
using erkulSale.data.Abstract;
using erkulSale.entity;

namespace erkulSale.business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public string ErrorMessage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void AddCategory(Category category)
        {
            _categoryRepository.AddCategory(category);
        }

        public void AddProductToCategory(int productId, int categoryId)
        {
            _categoryRepository.AddProductToCategory(productId, categoryId);

        }

        public void Create(Category entity)
        {
            _categoryRepository.Create(entity);
        }

        public void Delete(Category entity)
        {
            _categoryRepository.Delete(entity);
        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.DeleteCategory(id);

        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _categoryRepository.DeleteFromCategory(productId, categoryId);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();

        }

        public Category GetById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _categoryRepository.GetByIdWithProducts(categoryId);
        }

        public List<Category> GetCategoriesByName(string name)
        {
            return _categoryRepository.GetCategoriesByName(name);

        }

        public List<Category> GetCategoriesByProductId(int id)
        {
            return _categoryRepository.GetCategoriesByProductId(id);
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetCategoryById(id);

        }

        public Category GetCategoryByUrl(string url)
        {
            return _categoryRepository.GetCategoryByUrl(url);

        }

        public Category GetCategoryWithProducts(int categoryId)
        {
            return _categoryRepository.GetCategoryWithProducts(categoryId);
        }

        public void Update(Category entity)
        {
            _categoryRepository.Update(entity);
        }

        public void UpdateCategoryMethod(int id, string name, string url)
        {
            _categoryRepository.UpdateCategoryMethod(id, name, url);

        }

        public bool Validation(Category entity)
        {
            throw new System.NotImplementedException();
        }
    }
}