using System.Text.Json.Serialization;

namespace DataAccess.Models;

public partial class OrderItem
{
    public Guid OrderItemId { get; set; }

    public Guid? OrderId { get; set; }

    public Guid? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public Guid? SizeId { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [JsonIgnore]
    public virtual Order? Order { get; set; }

    [JsonIgnore]
    public virtual Size? Size { get; set; }
}
