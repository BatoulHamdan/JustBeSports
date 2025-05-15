using JustBeSports.Core.Entities;
using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace JustBeSports.Core.Services
{
    public class ProductService : IProductService
    {
        #region Fields

        private readonly ProductFeatures _productFeatures;

        #endregion

        #region Constructors

        public ProductService(ProductFeatures productFeatures)
        {
            _productFeatures = productFeatures;
        }

        #endregion

        #region Methods

        public List<ProductModel> GetListOfProducts()
        {
            return _productFeatures.GetAll()
                .Select(product => new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Color = product.Color,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Images = product.ProductImages.Select(i => new ProductImageModel
                    {
                        Id = i.Id,
                        Url = i.Url,
                    }).ToList(),
                    Variants = product.ProductVariants.Select(variant => new ProductVariantModel
                    {
                        Id = variant.Id,
                        Stock = variant.Stock,
                        Size = variant.Size,  
                    }).ToList()
                })
                .ToList();
        }

        public ProductModel GetProductById(int id)
        {
            var product = _productFeatures.GetById(id);
            if (product == null) return null;

            return new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Color = product.Color,
                CategoryId = product.CategoryId,
                Images = product.ProductImages.Select(i => new ProductImageModel
                {
                    Id = i.Id,
                    Url = i.Url,
                }).ToList(),
                Variants = product.ProductVariants.Select(v => new ProductVariantModel
                {
                    Id = v.Id,
                    Stock = v.Stock,
                    Size = v.Size,   
                }).ToList()
            };
        }

        public ProductModel GetProductByName(string name)
        {
            return _productFeatures.GetByName(name);
        }

        public List<ProductModel> GetProductsByCategoryId(int categoryId)
        {
            return _productFeatures.GetByCategoryId(categoryId)
                .Select(product => new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Color = product.Color,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Images = product.ProductImages.Select(i => new ProductImageModel
                    {
                        Id = i.Id,
                        Url = i.Url,
                    }).ToList(),
                    Variants = product.ProductVariants.Select(variant => new ProductVariantModel
                    {
                        Id = variant.Id,
                        Stock = variant.Stock,
                        Size = variant.Size,
                    }).ToList()
                })
                .ToList();
        }

        public int AddProduct(string name, string description, decimal price, string color, int categoryId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                CategoryId = categoryId,
                Price = price,
                Color = color
            };

            _productFeatures.Insert(product); 

            return product.Id; 
        }


        public void UpdateProduct(int id, string name, string description, decimal price, string color, int categoryId)
        {
            var product = _productFeatures.GetById(id);
            if (product != null)
            {
                product.Name = name;
                product.Description = description;
                product.Price = price;
                product.Color = color;
                product.CategoryId = categoryId;
                _productFeatures.Update(product);
            }
        }

        public void DeleteProduct(int id)
        {
            var product = _productFeatures.GetById(id);
            if (product != null)
            {
                _productFeatures.Delete(id);
            }
        }

        public List<ProductModel> SearchProducts(string? query, int? categoryId)
        {
            return _productFeatures.SearchProducts(query, categoryId)
                .Select(product => new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Color = product.Color,
                    CategoryId = product.CategoryId,
                    Images = product.ProductImages.Select(i => new ProductImageModel
                    {
                        Id = i.Id,
                        Url = i.Url,
                    }).ToList(),
                    Variants = product.ProductVariants.Select(variant => new ProductVariantModel
                    {
                        Id = variant.Id,
                        Stock = variant.Stock,
                        Size = variant.Size,
                    }).ToList()
                })
                .ToList();
        }

        #endregion
    }
}
