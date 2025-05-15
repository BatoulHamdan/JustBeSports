using JustBeSports.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JustBeSports.Models
{
    public class ProductFilterViewModel
    {
        public string? Query { get; set; }
        public int? SelectedCategoryId { get; set; }

        public List<Category> Categories { get; set; } = new();
        public List<Product> Products { get; set; } = new();
    }


}
