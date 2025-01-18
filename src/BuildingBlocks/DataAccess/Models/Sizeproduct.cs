using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class SizeProduct
{
    public Guid SizeProductId { get; set; }

    public Guid? ProductId { get; set; }

    public int? SizeQuantity { get; set; }

    public Guid? SizeId { get; set; }

    public string? SizeName { get; set; }

    [JsonIgnore]
    public virtual Product? Product { get; set; }

    [JsonIgnore]
    public virtual Size? Size { get; set; }
}
