namespace JustBeSports.Core.Entities;

public partial class ProductImage
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
