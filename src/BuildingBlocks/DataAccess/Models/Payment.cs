using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Payment
{
    [Key]
    public Guid PaymentId { get; set; }

    public decimal? Amount { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public string? TransactionId { get; set; }

    public Guid? UserId { get; set; }

    public virtual PaymentSubscription Payment1 { get; set; } = null!;

    public virtual Order PaymentNavigation { get; set; } = null!;

    public virtual User? User { get; set; }
}
