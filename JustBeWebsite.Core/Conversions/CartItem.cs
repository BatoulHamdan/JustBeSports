using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class CartItem
    {
        public static implicit operator CartItemModel(CartItem item)
        {
            CartItemModel retValue = null;
            if (item != null)
            {
                retValue = new CartItemModel
                {
                    Id = item.Id,
                    SessionId = item.SessionId,
                    ProductId = item.ProductId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    OrderId = item.OrderId,
                    Product = item.Product,
                    ProductVariant = item.ProductVariant,
                };
            }
            return retValue;
        }

        public static implicit operator CartItem(CartItemModel item)
        {
            CartItem retValue = null;
            if (item != null)
            {
                retValue = new CartItem
                {
                    Id = item.Id,
                    SessionId = item.SessionId,
                    ProductId = item.ProductId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,   
                    OrderId = item.OrderId,
                    Product = item.Product,
                    ProductVariant = item.ProductVariant,
                };
            }
            return retValue;
        }
    }
}
