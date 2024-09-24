using System;
using System.Collections.Generic;

namespace EventBus.Models;

public partial class Feedback
{
    public Guid FeedbackId { get; set; }

    public Guid? OrderItemId { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }

    public DateOnly? Date { get; set; }
}
