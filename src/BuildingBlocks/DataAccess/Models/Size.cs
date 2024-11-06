using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class Size
{
    public Guid SizeId { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [JsonIgnore]
    public virtual ICollection<SizeProduct> SizeProducts { get; set; } = new List<SizeProduct>();
}
