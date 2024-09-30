﻿using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Category
{
    [Key]
    public Guid CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
