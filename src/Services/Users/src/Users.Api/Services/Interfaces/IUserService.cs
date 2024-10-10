using DataAccess.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Users.Api.Dtos.Request;

namespace Users.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(CreateUserDTO model);
        Task<User> UpdateUser(UpdateUsetDTO model);
        Task<bool> DeleteUser(string id);
    }
}
