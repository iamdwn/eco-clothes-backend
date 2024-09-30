using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Size
{
    [Key]
    public Guid SizeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<SizeProduct> SizeProducts { get; set; } = new List<SizeProduct>();
}
