using JustBeSports.Core.Context;
using JustBeSports.Core.Entities;
using JustBeSports.Core.Features;
using JustBeSports.Core.Interfaces;
using JustBeSports.Core.Models;

namespace JustBeSports.Core.Services
{
    public class OrderService : IOrderService
    {
        #region Fields

        private readonly OrderFeatures _orderFeatures;
        private readonly CartItemFeatures _cartItemFeatures;
        private readonly JustBeSportsDbContext _context;

        #endregion

        #region Constructors

        public OrderService(OrderFeatures orderFeatures, CartItemFeatures cartItemFeatures, JustBeSportsDbContext context)
        {
            _orderFeatures = orderFeatures;
            _cartItemFeatures = cartItemFeatures;
            _context = context; 
        }

        #endregion

        #region Methods

        public List<OrderModel> GetListOfOrders()
        {
            return _orderFeatures.GetAll()
                .Select(x => (OrderModel)x)
                .ToList();
        }

        public OrderModel GetOrderById(int id)
        {
            return _orderFeatures.GetById(id);
        }

        public OrderModel AddOrder(string sessionId, string firstName, string lastName, string governate, string instagramAccount, string phoneNumber, string fullAddress, decimal totalPrice)
        {
            var cartItems = _cartItemFeatures.GetBySessionId(sessionId);

            if (cartItems == null || !cartItems.Any())
                throw new Exception("Cart is empty.");

            foreach (var cartItem in cartItems)
            {
                if (cartItem.ProductVariant == null)
                    throw new Exception("Product variant is missing.");

                if (cartItem.ProductVariant.Stock < cartItem.Quantity)
                    throw new Exception("Not enough stock for product.");

                cartItem.ProductVariant.Stock -= cartItem.Quantity;
                _cartItemFeatures.Update(cartItem);
            }

            var order = new Order
            {
                SessionId = sessionId,
                FirstName = firstName,
                LastName = lastName,
                Governate = governate,
                InstagramAccount = instagramAccount,
                PhoneNumber = phoneNumber,
                FullAddress = fullAddress,
                TotalPrice = totalPrice,
                OrderDate = DateTime.Now,
                Status = "Pending",
                CartItems = cartItems
            };

            _orderFeatures.Insert(order);

            foreach (var cartItem in order.CartItems)
            {
                cartItem.OrderId = order.Id;
                _cartItemFeatures.Update(cartItem);
            }

            return order;
        }

        public void UpdateOrder(int id, string firstName, string lastName, string governate, string instagramAccount, string phoneNumber, string fullAddress, decimal totalPrice)
        {
            var orderModel = _orderFeatures.GetById(id);
            if (orderModel != null)
            {
                orderModel.FirstName = firstName;
                orderModel.LastName = lastName;
                orderModel.Governate = governate;
                orderModel.InstagramAccount = instagramAccount;
                orderModel.PhoneNumber = phoneNumber;
                orderModel.FullAddress = fullAddress;
                orderModel.TotalPrice = totalPrice;
                _orderFeatures.Update(orderModel);
            }
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            var orderModel = _orderFeatures.GetById(orderId);
            if (orderModel != null)
            {
                orderModel.Status = newStatus;
                _orderFeatures.Update(orderModel);
            }
        }

        public void DeleteOrder(int id)
        {
            var orderModel = _orderFeatures.GetById(id);
            if (orderModel != null)
            {
                _orderFeatures.Delete(id);
            }
        }

        public OrderConfirmationModel ConfirmOrder(OrderModel order)
        {
            return new OrderConfirmationModel
            {
                OrderId = order.Id,
                FullName = $"{order.FirstName} {order.LastName}",
                PhoneNumber = order.PhoneNumber,
                FullAddress = order.FullAddress,
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,

                CartItems = order.CartItems.Select(cartItem => new CartItemModel
                {
                    Id = cartItem.Id,
                    SessionId = cartItem.SessionId,
                    ProductId = cartItem.ProductId,
                    ProductVariantId = cartItem.ProductVariantId,
                    Quantity = cartItem.Quantity,
                    OrderId = cartItem.OrderId,
                    Product = cartItem.Product,
                    ProductVariant = cartItem.ProductVariant
                }).ToList(),

                Message = "Thank you for your order! We will contact you soon."
            };
        }

        #endregion
    }
}
