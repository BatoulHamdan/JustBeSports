using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Core.Features
{
    public class ProductVariantFeatures : BaseFeatures
    {
        #region Constructors

        public ProductVariantFeatures(JustBeSportsDbContext context) : base(context) 
        { 
        }

        #endregion

        #region Methods

        public List<ProductVariant> GetAll()
        {
            return _context.ProductVariants
                .ToList();
        }

        public ProductVariant GetById(int id)
        {
            return _context.ProductVariants
                .FirstOrDefault(x => x.Id == id);
        }

        public List<ProductVariant> GetByProductId(int productId)
        {
            return _context.ProductVariants
                .Where(x => x.ProductId == productId)
                .ToList();
        }

        public ProductVariant GetProductVariantBySize(string size)
        {
            return _context.ProductVariants
                .FirstOrDefault(x => x.Size == size);
        }

        public void Insert(ProductVariant productVariant)
        {
            var existingVariant = _context.ProductVariants
                .FirstOrDefault(x => x.ProductId == productVariant.ProductId && x.Size == productVariant.Size);

            if (existingVariant != null)
            {
                // If variant exists, increase stock
                existingVariant.Stock += productVariant.Stock;
            }
            else
            {
                // Otherwise, add new variant
                _context.ProductVariants.Add(productVariant);
            }

            _context.SaveChanges();
        }

        public void Update(ProductVariant ProductVariant)
        {
            var existingProductVariant = _context.ProductVariants.FirstOrDefault(x => x.Id == ProductVariant.Id);
            if (existingProductVariant != null)
            {
                existingProductVariant.Size = ProductVariant.Size;
                existingProductVariant.Stock = ProductVariant.Stock;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var productVariant = _context.ProductVariants.FirstOrDefault(x => x.Id == id);
            if (productVariant != null)
            {
                // Check if any CartItem is using this variant
                bool isUsedInCart = _context.CartItems.Any(c => c.ProductVariantId == id);
                if (isUsedInCart)
                {
                    throw new InvalidOperationException("Cannot delete this variant because it is used in a cart.");
                }

                _context.ProductVariants.Remove(productVariant);
                _context.SaveChanges();
            }
        }

        public void DeleteByProductId(int productId)
        {
            var variants = _context.ProductVariants.Where(v => v.ProductId == productId).ToList();

            foreach (var variant in variants)
            {
                bool isUsedInCart = _context.CartItems.Any(c => c.ProductVariantId == variant.Id);
                if (isUsedInCart)
                {
                    throw new InvalidOperationException($"Cannot delete variant with ID {variant.Id} because it is used in a cart.");
                }
            }

            _context.ProductVariants.RemoveRange(variants);
            _context.SaveChanges();
        }

        #endregion
    }
}
