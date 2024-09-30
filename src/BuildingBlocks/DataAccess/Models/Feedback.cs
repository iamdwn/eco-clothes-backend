using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public partial class Feedback
{
    [Key]
    public Guid FeedbackId { get; set; }

    public Guid? OrderItemId { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }

    public DateOnly? Date { get; set; }

    public virtual OrderItem? OrderItem { get; set; }
}
