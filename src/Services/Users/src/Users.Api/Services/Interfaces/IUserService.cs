using DataAccess.Models;
using Users.Api.Dtos.Request;

namespace Users.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDTO model);
    }
}
