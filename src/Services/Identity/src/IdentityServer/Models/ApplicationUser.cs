using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string ImgUrl { get; set; }
    }
}
