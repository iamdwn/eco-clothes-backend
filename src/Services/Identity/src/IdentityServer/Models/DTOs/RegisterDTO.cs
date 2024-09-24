using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        //public string Address { get; set; }
        public string ImgUrl { get; set; }
        public string RoleName { get; set; }
    }
}
