using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class ProductVariant
    {
        public static implicit operator ProductVariantModel(ProductVariant item)
        {
            ProductVariantModel retValue = null;
            if (item != null)
            {
                retValue = new ProductVariantModel
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Size = item.Size,
                    Stock = item.Stock
                };
            }
            return retValue;
        }

        public static implicit operator ProductVariant(ProductVariantModel item)
        {
            ProductVariant retValue = null;
            if (item != null)
            {
                retValue = new ProductVariant
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Size = item.Size,
                    Stock = item.Stock
                };
            }
            return retValue;
        }
    }
}
