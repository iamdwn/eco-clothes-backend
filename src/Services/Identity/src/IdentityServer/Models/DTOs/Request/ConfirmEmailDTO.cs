namespace IdentityServer.Models.DTOs.Request
{
    public class ConfirmEmailDTO
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}
