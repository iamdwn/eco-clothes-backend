using AutoMapper;
using DataAccess.Base;
using DataAccess.Models;
using System.Data;
using Users.Api.Dtos.Request;
using Users.Api.Services.Interfaces;

namespace Users.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<User> CreateUser(CreateUserDTO model)
        {
            try
            {
                var user = _mapper.Map<User>(model);

                var existingUser = _unitOfWork.UserRepository.GetByID(user.UserId);
                if (existingUser != null)
                {
                    throw new DuplicateNameException($"User with email {user.Email} is exist.");
                }

                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();

                return Task.FromResult(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<User> UpdateUser(UpdateUsetDTO model)
        {
            try
            {
                var existingUser = _unitOfWork.UserRepository.GetByID(model.UserId);
                if (existingUser == null) return null;

                existingUser.Phone = model.Phone;
                existingUser.ImgUrl = model.ImgUrl;
                existingUser.FullName = model.FullName;

                _unitOfWork.UserRepository.Update(existingUser);
                _unitOfWork.Save();

                return Task.FromResult(existingUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<bool> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}
