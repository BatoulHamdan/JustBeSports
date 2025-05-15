using JustBeSports.Core.Models;

namespace JustBeSports.Core.Interfaces
{
    public interface IOrderService
    {
        #region Methods

        List<OrderModel> GetListOfOrders();

        OrderModel GetOrderById(int id);

        OrderModel AddOrder(string sessionId, string firstName, string lastName, string governate, string instagramAccount, string phoneNumber, string fullAddress, decimal totalPrice);

        void UpdateOrder(int id, string firstName, string lastName, string governate, string instagramAccount, string phoneNumber, string fullAddress, decimal totalPrice);

        void UpdateOrderStatus(int orderId, string newStatus);

        void DeleteOrder(int id);

        OrderConfirmationModel ConfirmOrder(OrderModel order);

        #endregion
    }
}
