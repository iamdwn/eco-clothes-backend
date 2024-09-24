using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
