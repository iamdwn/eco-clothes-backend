using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid? UserId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public Guid? PaymentId { get; set; }

    public string? Address { get; set; }
}
