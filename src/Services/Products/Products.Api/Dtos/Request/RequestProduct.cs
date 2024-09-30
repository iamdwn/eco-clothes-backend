namespace Products.Api.Dtos.Request
{
    public class RequestProduct
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        //public int? NumberOfSold { get; set; }

        public string? ImgUrl { get; set; }

        public string? Description { get; set; }

        public List<SizeDto>? Sizes { get; set; }

        public List<CategoryDto>? Categories { get; set; }
    }
}
