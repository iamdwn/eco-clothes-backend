using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Sizeproduct
{
    public Guid SizeProductId { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? SizeId { get; set; }
}
