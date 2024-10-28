namespace DataAccess.Models;

public partial class Cart
{
    public Guid CartId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }

    public virtual Size? Size { get; set; }
}
