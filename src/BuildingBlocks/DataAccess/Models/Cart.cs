using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Cart
{
    public Guid CartId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    public int? Quantity { get; set; }
}
