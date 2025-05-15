namespace JustBeSports.Core.Models
{
    public class ProductVariantModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Size { get; set; } = null!;

        public int Stock { get; set; }
    }
}
