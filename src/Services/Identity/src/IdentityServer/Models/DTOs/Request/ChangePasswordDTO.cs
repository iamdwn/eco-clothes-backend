using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.DTOs.Request
{
    public class ChangePasswordDTO
    {
        public string OldPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
