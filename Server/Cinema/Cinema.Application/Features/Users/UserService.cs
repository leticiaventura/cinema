using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using Cinema.Infra.Crypto;

namespace Cinema.Application.Features.Users
{
    public class UserService : AbstractService<User>, IUserService
    {
        public UserService(IUserRepository repository, Mapper mapper) : base(repository, mapper)
        {
        }

        public bool IsEmailAlreadyInUse(string email, long userId)
        {
            return ((IUserRepository)_repository).IsEmailAlreadyInUse(email, userId);
        }

        public User Login(string email, string password)
        {
            password = password.GenerateHash();
            return ((IUserRepository)_repository).GetByCredentials(email, password);
        }
    }
}
