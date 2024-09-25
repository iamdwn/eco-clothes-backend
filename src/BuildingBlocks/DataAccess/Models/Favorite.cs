using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public string? UserId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
