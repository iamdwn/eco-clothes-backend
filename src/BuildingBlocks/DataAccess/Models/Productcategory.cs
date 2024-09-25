﻿using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class Productcategory
{
    public Guid ProductCategoryId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CategoryId { get; set; }

    [JsonIgnore]
    public virtual Category? Category { get; set; }

    [JsonIgnore]
    public virtual Product? Product { get; set; }
}
