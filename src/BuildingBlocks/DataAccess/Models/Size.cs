using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class Size
{
    public Guid SizeId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<SizeProduct> SizeProducts { get; set; } = new List<SizeProduct>();
}
