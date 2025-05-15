using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface IProductService
    {
        #region Methods

        List<ProductModel> GetListOfProducts();

        ProductModel GetProductById(int id);

        ProductModel GetProductByName(string name);

        List<ProductModel> GetProductsByCategoryId(int categoryId);

        int AddProduct(string name, string description, decimal price, string color, int categoryId);

        void UpdateProduct(int id, string name, string description, decimal price, string color, int categoryId);

        void DeleteProduct(int id);

        List<ProductModel> SearchProducts(string? query, int? categoryId);

        #endregion
    }
}
