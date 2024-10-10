using AutoMapper;
using DataAccess.Base;
using DataAccess.Models;
using Users.Api.Dtos.Request;
using Users.Api.Services.Interfaces;

namespace Users.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<User> CreateUser(CreateUserDTO model)
        {
            var user = _mapper.Map<User>(model);
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
            return Task.FromResult(user);
        }
    }
}
