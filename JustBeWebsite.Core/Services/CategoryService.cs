using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;

namespace JustBeSports.Core.Services
{
    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly CategoryFeatures _categoryFeatures;

        #endregion

        #region Constructors

        public CategoryService(CategoryFeatures categoryFeatures)
        {
            _categoryFeatures = categoryFeatures;
        }

        #endregion

        #region Methods

        public List<CategoryModel> GetListOfCategories()
        {
            return _categoryFeatures.GetAll()
                .Select(x => (CategoryModel)x)
                .ToList();
        }

        public CategoryModel GetCategoryById(int id)
        {
            return _categoryFeatures.GetById(id);
        }

        public CategoryModel GetCategoryByName(string name)
        {
            return _categoryFeatures.GetByName(name);   
        }

        public void AddCategory(string name, string description)
        {
            CategoryModel category = new CategoryModel();
            category.Name = name;   
            category.Description = description;
            _categoryFeatures.Insert(category);
        }

        public void UpdateCategory(int id, string name, string description)
        {
            var category = _categoryFeatures.GetById(id);
            if(category != null)
            {
                category.Name = name;   
                category.Description = description;
                _categoryFeatures.Update(category);
            }
        }

        public void DeleteCategory(int id)
        {
            var category = _categoryFeatures.GetById(id);
            if (category != null)
            {
                _categoryFeatures.Delete(id);
            }
        }

        #endregion
    }
}
