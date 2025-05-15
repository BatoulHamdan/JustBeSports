using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Core.Features
{
    public class ProductFeatures : BaseFeatures
    {
        #region Constructors

        public ProductFeatures(JustBeSportsDbContext context) : base(context)
        {
        }

        #endregion

        #region Methods

        public List<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.ProductImages)   
                .Include(p => p.ProductVariants) 
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.ProductVariants)
                .FirstOrDefault(x => x.Id == id);
        }

        public Product GetByName(string name)
        {
            return _context.Products
                .FirstOrDefault(x => x.Name == name);
        }

        public List<Product> GetByCategoryId(int categoryId)
        {
            return _context.Products
                .Where(x => x.CategoryId == categoryId)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .ToList();
        }

        public void Insert(Product Product)
        {
            _context.Products.Add(Product);
            _context.SaveChanges();
        }

        public void Update(Product Product)
        {
            var existingProduct = _context.Products.FirstOrDefault(x => x.Id == Product.Id);
            if (existingProduct != null)
            {
                existingProduct.Name = Product.Name;
                existingProduct.Description = Product.Description;
                existingProduct.Price = Product.Price;
                existingProduct.Color = Product.Color;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductVariants)   
                .Include(p => p.ProductImages)     
                .FirstOrDefault(x => x.Id == id);

            if (product != null)
            {
                foreach (var variant in product.ProductVariants.ToList())
                {
                    _context.ProductVariants.Remove(variant);
                }

                foreach (var image in product.ProductImages.ToList())
                {
                    _context.ProductImages.Remove(image);
                }

                _context.Products.Remove(product);

                _context.SaveChanges();
            }
        }

        public List<Product> SearchProducts(string? query, int? categoryId)
        {
            var products = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                products = products.Where(p => p.Name.Contains(query));
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            return products.ToList();
        }


        #endregion    
    }
}
