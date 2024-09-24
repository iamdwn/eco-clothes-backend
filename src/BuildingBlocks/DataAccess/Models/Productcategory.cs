using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Productcategory
{
    public Guid ProductCategoryId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CategoryId { get; set; }
}
