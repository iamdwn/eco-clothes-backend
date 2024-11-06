namespace Favorites.Api.Dtos.Request
{
    public class RemoveProductFromFavoriteDTO
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
