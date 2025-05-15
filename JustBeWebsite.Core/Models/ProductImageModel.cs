namespace JustBeSports.Core.Models
{
    public class ProductImageModel
    {
        public int Id { get; set; }

        public string Url { get; set; } = null!;

        public int ProductId { get; set; }
    }
}
