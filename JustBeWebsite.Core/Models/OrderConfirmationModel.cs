namespace JustBeSports.Core.Models
{
    public class OrderConfirmationModel
    {
        public int OrderId { get; set; }

        public string FullName { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public string Message { get; set; }

        public string FullAddress { get; set; }
        
        public string PhoneNumber { get; set; }

        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}

