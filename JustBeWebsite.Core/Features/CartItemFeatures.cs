using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Core.Features
{
    public class CartItemFeatures : BaseFeatures
    {
        #region Constructors

        public CartItemFeatures(JustBeSportsDbContext context) : base(context)
        {
        }

        #endregion

        #region Methods

        public List<CartItem> GetBySessionId(string sessionId)
        {
            return _context.CartItems
                .Where(x => x.SessionId == sessionId && x.OrderId == null)
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(c => c.ProductVariant)
                .ToList();
        }

        public CartItem GetById(int id)
        {
            return _context.CartItems
                .Include(c => c.Product) 
                .FirstOrDefault(x => x.Id == id);
        }

        public void Insert(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();
        }

        public void Update(CartItem cartItem)
        {
            var existingCartItem = _context.CartItems.FirstOrDefault(x => x.Id == cartItem.Id);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity = cartItem.Quantity; 
                existingCartItem.OrderId = cartItem.OrderId;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(x => x.Id == id);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
        }

        #endregion
    }
}