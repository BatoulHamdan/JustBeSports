using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class Product
    {
        public static implicit operator ProductModel(Product item)
        {
            ProductModel retValue = null;
            if (item != null)
            {
                retValue = new ProductModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Color = item.Color,
                    CategoryId = item.CategoryId,
                };
            }
            return retValue;
        }

        public static implicit operator Product(ProductModel item)
        {
            Product retValue = null;
            if (item != null)
            {
                retValue = new Product
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Color = item.Color,
                    CategoryId = item.CategoryId,
                };
            }
            return retValue;
        }
    }
}
