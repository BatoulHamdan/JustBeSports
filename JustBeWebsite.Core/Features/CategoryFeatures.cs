using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Core.Features
{
    public class CategoryFeatures : BaseFeatures
    {
        #region Constructors

        public CategoryFeatures(JustBeSportsDbContext context) : base(context)
        {
        }

        #endregion

        #region Methods

        public List<Category> GetAll()
        {
            return _context.Categories
                .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                .FirstOrDefault(x => x.Id == id);
        }

        public Category GetByName(string name)
        {
            return _context.Categories
                .FirstOrDefault(x => x.Name == name);
        }

        public void Insert(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            var existingCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existingCategory != null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Include(c => c.Products)
                                              .FirstOrDefault(x => x.Id == id);
            if (category != null)
            {
                foreach (var product in category.Products.ToList())
                {
                    _context.Products.Remove(product);
                }

                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }


        #endregion
    }
}
