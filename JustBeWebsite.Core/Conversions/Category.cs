using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class Category
    {
        public static implicit operator CategoryModel(Category item)
        {
            CategoryModel retValue = null;
            if (item != null)
            {
                retValue = new CategoryModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                };
            }
            return retValue;
        }

        public static implicit operator Category(CategoryModel item)
        {
            Category retValue = null;
            if (item != null)
            {
                retValue = new Category
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                };
            }
            return retValue;
        }
    }
}
