namespace Favorites.Api.Dtos.Request
{
    public class AddProductToFavoriteDTO
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
