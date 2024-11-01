using AutoMapper;
using IdentityServer.Models;
using IdentityServer.Models.DTOs.Request;
using IdentityServer.Models.DTOs.Response;

namespace IdentityServer.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, CurrentUserDTO>();
        }
    }
}
