namespace JustBeSports.Core.Entities
{
    public partial class Order
    {
        public int Id { get; set; }

        public string SessionId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Governate { get; set; } = null!;

        public string? InstagramAccount { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string FullAddress { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Pending";

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
