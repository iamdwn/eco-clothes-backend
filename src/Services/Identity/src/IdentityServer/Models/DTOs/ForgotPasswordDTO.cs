using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
