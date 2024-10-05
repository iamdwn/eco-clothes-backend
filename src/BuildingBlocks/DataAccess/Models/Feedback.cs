using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class Feedback
{
    public Guid FeedbackId { get; set; }

    public Guid? OrderItemId { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }

    public DateOnly? Date { get; set; }

    [JsonIgnore]
    public virtual OrderItem? OrderItem { get; set; }
}
