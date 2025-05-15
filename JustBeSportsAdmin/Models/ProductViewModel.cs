using JustBeSports.Core.Models;

namespace JustBeSports.Core.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public List<ProductImageModel> Images { get; set; } = new();

        public List<ProductVariantModel> Variants { get; set; } = new();
    }
}
