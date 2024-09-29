using Microsoft.AspNetCore.Identity;
using DataAccess.Interfaces;

namespace IdentityServer.Models
{
    public class ApplicationUser : IdentityUser, ISoftDelete
    {
        public string FullName { get; set; }
        public string ImgUrl { get; set; }
        public bool IsDeleted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? DeletedWhen { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
