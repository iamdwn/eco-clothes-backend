using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public decimal? Amount { get; set; }

    public string? Method { get; set; }

    public string? Status { get; set; }

    public string? TransactionId { get; set; }

    public Guid? UserId { get; set; }

    public virtual PaymentSubscription PaymentNavigation { get; set; } = null!;

    public virtual User? User { get; set; }
}
