using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Size
{
    public Guid SizeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<SizeProduct> SizeProducts { get; set; } = new List<SizeProduct>();
}
