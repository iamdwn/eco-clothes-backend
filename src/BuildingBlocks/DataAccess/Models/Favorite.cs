using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Favorite
{
    [Key]
    public Guid FavoriteId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
