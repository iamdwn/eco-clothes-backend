using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Subscription
{
    [Key]
    public Guid SubscriptionId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public DateOnly? Period { get; set; }

    public virtual PaymentSubscription? PaymentSubscription { get; set; }

    public virtual User SubscriptionNavigation { get; set; } = null!;
}
