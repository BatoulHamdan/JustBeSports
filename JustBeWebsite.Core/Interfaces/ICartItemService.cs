using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface ICartItemService
    {
        #region Methods

        List<CartItemModel> GetListOfCartItems(string sessionId);

        CartItemModel GetCartItemById(int id);

        void AddCartItem(string sessionId, int productId, int productVariantId, int quantity);

        int GetQuantityInCart(string sessionId, int productVariantId);

        void UpdateCartItem(int id, int quantity, int? orderId);

        void DeleteCartItem(int id);

        #endregion
    }
}
