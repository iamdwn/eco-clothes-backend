using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class OrderItem
{
    [Key]
    public Guid OrderItemId { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
