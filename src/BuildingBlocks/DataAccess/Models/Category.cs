using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
