using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface ICategoryService
    {
        #region Methods

        List<CategoryModel> GetListOfCategories();

        CategoryModel GetCategoryById(int id);

        CategoryModel GetCategoryByName(string name);

        void AddCategory(string name, string description);

        void UpdateCategory(int id, string name, string description);

        void DeleteCategory(int id);

        #endregion
    }
}
