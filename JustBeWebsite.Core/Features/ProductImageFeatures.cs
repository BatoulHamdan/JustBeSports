using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;

namespace JustBeSports.Core.Features
{
    public class ProductImageFeatures : BaseFeatures
    {
        #region Constructors

        public ProductImageFeatures(JustBeSportsDbContext context) : base(context)
        {
        }

        #endregion

        #region Methods

        public List<ProductImage> GetAll()
        {
            return _context.ProductImages
                .ToList();
        }

        public ProductImage GetById(int id)
        {
            return _context.ProductImages
                .FirstOrDefault(x => x.Id == id);
        }

        public List<ProductImage> GetByUrl(string url)
        {
            return _context.ProductImages
                .ToList();
        }

        public List<ProductImage> GetByProductId(int productId)
        {
            return _context.ProductImages
                .Where(x => x.ProductId == productId)
                .ToList();
        }

        public void Insert(ProductImage ProductImage)
        {
            _context.ProductImages.Add(ProductImage);
            _context.SaveChanges();
        }

        public void Update(ProductImage ProductImage)
        {
            var existingProductImage = _context.ProductImages.FirstOrDefault(x => x.Id == ProductImage.Id);
            if (existingProductImage != null)
            {
                existingProductImage.Url = ProductImage.Url;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var ProductImage = _context.ProductImages.FirstOrDefault(x => x.Id == id);
            if (ProductImage != null)
            {
                _context.ProductImages.Remove(ProductImage);
                _context.SaveChanges();
            }
        }

        #endregion
    }
}
