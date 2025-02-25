﻿namespace DataAccess.Models;

public partial class Subscription
{
    public Guid SubscriptionId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public DateOnly? Period { get; set; }
}
