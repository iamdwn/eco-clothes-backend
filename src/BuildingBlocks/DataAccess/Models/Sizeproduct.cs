using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class SizeProduct
{
    public Guid SizeProductId { get; set; }

    public Guid? ProductId { get; set; }

    public int? SizeQuantity { get; set; }

    public Guid? SizeId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Size? Size { get; set; }
}
