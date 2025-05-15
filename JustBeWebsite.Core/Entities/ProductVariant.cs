namespace JustBeSports.Core.Entities;

public partial class ProductVariant
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Size { get; set; } = null!;

    public int Stock { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Product Product { get; set; } = null!;
}

