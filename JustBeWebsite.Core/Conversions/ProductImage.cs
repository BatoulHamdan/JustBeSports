using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class ProductImage
    {
        public static implicit operator ProductImageModel(ProductImage item)
        {
            ProductImageModel retValue = null;
            if (item != null)
            {
                retValue = new ProductImageModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Url = item.Url,
                };
            }
            return retValue;
        }

        public static implicit operator ProductImage(ProductImageModel item)
        {
            ProductImage retValue = null;
            if (item != null)
            {
                retValue = new ProductImage
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Url = item.Url,
                };
            }
            return retValue;
        }
    }
}
