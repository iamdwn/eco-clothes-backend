using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Order
{
    [Key]
    public Guid OrderId { get; set; }

    public Guid? UserId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public Guid? PaymentId { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual User? User { get; set; }
}
