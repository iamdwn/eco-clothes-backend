using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
