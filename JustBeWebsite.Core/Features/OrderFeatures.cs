using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace JustBeSports.Core.Features
{
    public class OrderFeatures : BaseFeatures
    {
        #region Constructors

        public OrderFeatures(JustBeSportsDbContext context) : base(context)
        {
        }

        #endregion

        #region Methods

        public List<Order> GetAll()
        {
            return _context.Orders
                .ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.ProductVariant)
                .FirstOrDefault(o => o.Id == id);
        }

        public void Insert(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(x => x.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.FirstName = order.FirstName;
                existingOrder.LastName = order.LastName;
                existingOrder.Governate = order.Governate;
                existingOrder.InstagramAccount = order.InstagramAccount;
                existingOrder.PhoneNumber = order.PhoneNumber;
                existingOrder.FullAddress = order.FullAddress;
                existingOrder.TotalPrice = order.TotalPrice;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Include(o => o.CartItems)
                                       .FirstOrDefault(x => x.Id == id);
            if (order != null)
            {
                foreach (var item in order.CartItems.ToList())
                {
                    _context.CartItems.Remove(item);
                }

                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        #endregion
    }
}
