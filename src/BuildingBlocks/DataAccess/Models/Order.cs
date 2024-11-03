namespace DataAccess.Models;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid? UserId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public Guid? PaymentId { get; set; }

    public string? Address { get; set; }

    public string? Status { get; set; }

    public string? Username { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}
