namespace JustBeSports.Core.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Color { get; set; }

        public int CategoryId { get; set; }

        public List<ProductImageModel> Images { get; set; } = new(); 

        public List<ProductVariantModel> Variants { get; set; } = new();
    }
}
