using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class PaymentSubscription
{
    public Guid PaymentSubscriptionId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? SubscriptionId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? Price { get; set; }
}
