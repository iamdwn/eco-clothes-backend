namespace IdentityServer.Models.DTOs.Response
{
    public class CurrentUserDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string ImgUrl { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
    }
}
