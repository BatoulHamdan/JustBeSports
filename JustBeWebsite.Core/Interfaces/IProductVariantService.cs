using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface IProductVariantService
    {
        #region Methods

        List<ProductVariantModel> GetListOfProductVariants();

        ProductVariantModel GetProductVariant(int id);

        List<ProductVariantModel> GetProductVariantByProductId(int productId);

        ProductVariantModel GetProductVariantBySize(string size);

        void InsertProductVariant(int productId, string size, int stock);

        void UpdateProductVariant(int id, int productId, string size, int stock);

        void DeleteProductVariant(int id);

        void DeleteVariantsByProductId(int productId);
        #endregion
    }
}
