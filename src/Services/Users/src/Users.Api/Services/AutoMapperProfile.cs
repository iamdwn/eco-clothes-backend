using AutoMapper;
using DataAccess.Models;
using Users.Api.Dtos.Request;

namespace Users.Api.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDTO, User>();
        }
    }
}
