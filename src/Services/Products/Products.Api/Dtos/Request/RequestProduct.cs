namespace Products.Api.Dtos.Request
{
    public class RequestProduct
    {
        private string ProductName { get; set; } = null!;

        private decimal? OldPrice { get; set; }

        private decimal? NewPrice { get; set; }

        private int? NumberOfSold { get; set; }

        private string? ImgUrl { get; set; }

        private string? Description { get; set; }

        private List<SizeDto>? Sizes { get; set; }
    }
}
