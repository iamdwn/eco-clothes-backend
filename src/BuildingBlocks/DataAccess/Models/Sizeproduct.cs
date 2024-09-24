using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Sizeproduct
{
    public Guid SizeProductId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }
}
