using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Favorite
{
    public Guid FavoriteId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Product? Product { get; set; }
}
