using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Productcategory> Productcategories { get; set; } = new List<Productcategory>();
}
