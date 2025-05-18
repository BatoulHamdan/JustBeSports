using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface IProductImageService
    {
        #region Methods

        List<ProductImageModel> GetProductImages();

        ProductImageModel GetProductImage(int id);

        List<ProductImageModel> GetProductImagesByUrl(string url);

        List<ProductImageModel> GetProductImageByProductId(int productId);

        void InsertProductImage(string url, int productId);

        void UpdateProductImage(int id, string url, int productId);

        void DeleteProductImage(int id);

        #endregion
    }
}
