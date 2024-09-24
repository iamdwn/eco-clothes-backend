using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public int? NumberOfSold { get; set; }

    public string? ImgUrl { get; set; }

    public int? Amount { get; set; }

    public string? Description { get; set; }
}
