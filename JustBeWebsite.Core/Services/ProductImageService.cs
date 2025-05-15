using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;

namespace JustBeSports.Core.Services
{
    public class ProductImageService : IProductImageService
    {
        #region Fields

        private readonly ProductImageFeatures _productImageFeatures;

        #endregion

        #region Constructors

        public ProductImageService(ProductImageFeatures productImageFeatures)
        {
            _productImageFeatures = productImageFeatures;
        }

        #endregion

        #region Methods

        public List<ProductImageModel> GetProductImages()
        {
            return _productImageFeatures.GetAll()
                .Select(x => (ProductImageModel)x)
                .ToList();
        }

        public ProductImageModel GetProductImage(int id)
        {
            return _productImageFeatures.GetById(id);
        }

        public ProductImageModel GetProductImageByUrl(string url)
        {
            return _productImageFeatures.GetByUrl(url);
        }

        public List<ProductImageModel> GetProductImageByProductId(int productId)
        {
            return _productImageFeatures.GetByProductId(productId)
                .Select(x => (ProductImageModel)x)
                .ToList(); ;
        }

        public void InsertProductImage(string url, int productId)
        {
            ProductImageModel productImage = new ProductImageModel();
            productImage.Url = url;
            productImage.ProductId = productId;
            _productImageFeatures.Insert(productImage);
        }

        public void UpdateProductImage(int id, string url, int productId)
        {
            var productImage = _productImageFeatures.GetById(id);
            if (productImage != null)
            {
                productImage.Url = url;
                productImage.ProductId = productId;
                _productImageFeatures.Update(productImage);
            }
        }

        public void DeleteProductImage(int id)
        {
            var productImage = _productImageFeatures.GetById(id);
            if (productImage != null)
            {
                _productImageFeatures.Delete(id);
            }
        }

        #endregion
    }
}
