namespace JustBeSports.Core.Entities;

public partial class CartItem
{
    public int Id { get; set; }

    public string SessionId { get; set; } = null!;

    public int ProductId { get; set; }

    public int ProductVariantId { get; set; }

    public int Quantity { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ProductVariant ProductVariant { get; set; } = null!;
}
