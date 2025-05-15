using JustBeSports.Core.Entities;
using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;

namespace JustBeSports.Core.Services
{
    public class CartItemService : ICartItemService
    {
        #region Fields

        private readonly CartItemFeatures _cartItemFeatures;
        private readonly ProductVariantFeatures _productVariantFeatures;

        #endregion

        #region Constructors

        public CartItemService(CartItemFeatures cartItemFeatures, ProductVariantFeatures productVariantFeatures)
        {
            _cartItemFeatures = cartItemFeatures;
            _productVariantFeatures = productVariantFeatures;
        }

        #endregion

        #region Methods

        List<CartItemModel> ICartItemService.GetListOfCartItems(string sessionId)
        {
            return _cartItemFeatures.GetBySessionId(sessionId)
                .Select(x => (CartItemModel)x)
                .ToList();
        }

        CartItemModel ICartItemService.GetCartItemById(int id)
        {
            return _cartItemFeatures.GetById(id);
        }

        void ICartItemService.AddCartItem(string sessionId, int productId, int productVariantId, int quantity)
        {
            var existingItem = _cartItemFeatures.GetBySessionId(sessionId)
                .FirstOrDefault(c =>
                    c.ProductId == productId &&
                    c.ProductVariantId == productVariantId);

            if (existingItem != null)
            {
                var variant = _productVariantFeatures.GetById(productVariantId);
                if (existingItem.Quantity + quantity > variant.Stock)
                {
                    throw new InvalidOperationException("Not enough stock available.");
                }

                existingItem.Quantity += quantity;
                _cartItemFeatures.Update(existingItem);
            }
            else
            {
                CartItemModel cartItem = new CartItemModel
                {
                    SessionId = sessionId,
                    ProductId = productId,
                    ProductVariantId = productVariantId,
                    Quantity = quantity
                };
                _cartItemFeatures.Insert(cartItem);
            }
        }

        int ICartItemService.GetQuantityInCart(string sessionId, int productVariantId)
        {
            var existingItem = _cartItemFeatures.GetBySessionId(sessionId)
                .FirstOrDefault(c =>
                    c.ProductVariantId == productVariantId);

            return existingItem?.Quantity ?? 0;
        }


        void ICartItemService.UpdateCartItem(int id, int quantity, int? orderId)
        {
            var cartItem = _cartItemFeatures.GetById(id);
            if (cartItem != null)
            {  
                cartItem.Quantity = quantity;
                cartItem.OrderId = orderId;
                _cartItemFeatures.Update(cartItem);
            }
        }

        void ICartItemService.DeleteCartItem(int id)
        {
            var cartItem = _cartItemFeatures.GetById(id);
            if (cartItem != null)
            {
                _cartItemFeatures.Delete(id);
            }
        }

        #endregion
    }
}
