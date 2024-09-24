using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Productcategory
{
    public Guid ProductCategoryId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CategoryId { get; set; }
}
