namespace DataAccess.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public int? NumberOfSold { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }

    public Guid? UserId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<SizeProduct> SizeProducts { get; set; } = new List<SizeProduct>();

    public virtual User? User { get; set; }
}
