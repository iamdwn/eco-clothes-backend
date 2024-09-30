using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class PaymentSubscription
{
    [Key]
    public Guid PaymentSubscriptionId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? SubscriptionId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Price { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual Subscription? Subscription { get; set; }
}
