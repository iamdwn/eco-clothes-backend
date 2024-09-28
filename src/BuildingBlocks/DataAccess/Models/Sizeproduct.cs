using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class SizeProduct
{
    public int SizeProductId { get; set; }

    public int? ProductId { get; set; }

    public int? SizeId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Size? Size { get; set; }
}
