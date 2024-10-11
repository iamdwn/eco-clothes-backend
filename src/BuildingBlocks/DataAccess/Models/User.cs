using DataAccess.Interfaces;
using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class User : ISoftDelete
{
    public Guid UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public DateTime? DateCreated { get; set; }

    public Guid? SubscriptionId { get; set; }

    public string? ImgUrl { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedWhen { get; set; }
}
