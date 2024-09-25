using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public int? NumberOfSold { get; set; }

    public string? ImgUrl { get; set; }

    public int Amount { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Productcategory> Productcategories { get; set; } = new List<Productcategory>();

    public virtual ICollection<SizeProduct> SizeProducts { get; set; } = new List<SizeProduct>();
}
