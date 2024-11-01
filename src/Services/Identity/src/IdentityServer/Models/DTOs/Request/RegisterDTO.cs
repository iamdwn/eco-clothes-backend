using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.DTOs.Request
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password does not match with Password")]
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        //public string Address { get; set; }
        public string ImgUrl { get; set; }
        public string RoleName { get; set; }
    }
}
