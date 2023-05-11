using System.Collections.Generic;
using erkulSale.entity;

namespace erkulSale.data.Abstract
{
    public interface ICategoryRepository: IRepository<Category>
    {
        Category GetByIdWithProducts(int categoryId);

        void DeleteFromCategory(int productId,int categoryId);
        void AddCategory(Category category);
        List<Category> GetAllCategories();
        Category GetCategoryById(int id);
        Category GetCategoryByUrl(string url);
        List<Category> GetCategoriesByName(string name);
        List<Category> GetCategoriesByProductId(int id);
        void UpdateCategoryMethod(int id, string name, string url);
        void DeleteCategory(int id);
        void AddProductToCategory(int productId,int categoryId);
        Category GetCategoryWithProducts(int categoryId);
        









    }
}