using JustBeSports.Core.Models;

namespace JustBeSports.Core.Entities
{
    partial class Order
    {
        public static implicit operator OrderModel(Order item)
        {
            OrderModel retValue = null;
            if (item != null)
            {
                retValue = new OrderModel
                {
                    Id = item.Id,
                    SessionId = item.SessionId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Governate = item.Governate,
                    InstagramAccount = item.InstagramAccount,
                    PhoneNumber = item.PhoneNumber,
                    FullAddress = item.FullAddress,
                    OrderDate = item.OrderDate,
                    TotalPrice = item.TotalPrice,
                    Status = item.Status,
                    CartItems = item.CartItems,
                };
            }
            return retValue;
        }

        public static implicit operator Order(OrderModel item)
        {
            Order retValue = null;
            if (item != null)
            {
                retValue = new Order
                {
                    Id = item.Id,
                    SessionId = item.SessionId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Governate = item.Governate,
                    InstagramAccount = item.InstagramAccount,
                    PhoneNumber = item.PhoneNumber,
                    FullAddress = item.FullAddress,
                    OrderDate = item.OrderDate,
                    TotalPrice = item.TotalPrice,
                    Status = item.Status,
                    CartItems = item.CartItems,
                };
            }
            return retValue;
        }
    }
}
