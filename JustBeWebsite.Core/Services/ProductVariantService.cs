using JustBeSports.Core.Entities;
using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;

namespace JustBeSports.Core.Services
{
    public class ProductVariantService : IProductVariantService
    {
        #region Fields

        private readonly ProductVariantFeatures _productVariantFeatures;

        #endregion

        #region Constructors

        public ProductVariantService(ProductVariantFeatures productVariantFeatures)
        {
            _productVariantFeatures = productVariantFeatures;
        }

        #endregion

        #region Methods

        public List<ProductVariantModel> GetListOfProductVariants()
        {
            return _productVariantFeatures.GetAll()
                .Select(x => (ProductVariantModel)x)
                .ToList();
        }

        public ProductVariantModel GetProductVariant(int id)
        {
            return (ProductVariantModel)_productVariantFeatures.GetById(id);
        }

        public List<ProductVariantModel> GetProductVariantByProductId(int productId)
        {
            var variants = _productVariantFeatures.GetByProductId(productId);
            return variants.Select(x => (ProductVariantModel)x).ToList();
        }

        public ProductVariantModel GetProductVariantBySize(string size)
        {
            return (ProductVariantModel)_productVariantFeatures.GetProductVariantBySize(size);
        }

        public void InsertProductVariant(int productId, string size, int stock)
        {
            var productVariant = new ProductVariant
            {
                ProductId = productId,
                Size = size,
                Stock = stock
            };

            _productVariantFeatures.Insert(productVariant);
        }

        public void UpdateProductVariant(int id, int productId, string size, int stock)
        {
            var productVariant = new ProductVariant
            {
                Id = id,
                ProductId = productId,
                Size = size,
                Stock = stock
            };

            _productVariantFeatures.Update(productVariant);
        }

        public void DeleteProductVariant(int id)
        {
            _productVariantFeatures.Delete(id);
        }

        public void DeleteVariantsByProductId(int productId)
        {
            _productVariantFeatures.DeleteByProductId(productId);
        }

        #endregion
    }
}
