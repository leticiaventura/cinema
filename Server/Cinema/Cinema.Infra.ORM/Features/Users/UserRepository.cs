using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using Cinema.Infra.ORM.Base;

namespace Cinema.Infra.ORM.Features.Users
{
    public class UserRepository : AbstractRepository<User>, IUserRepository
    {
        public UserRepository(BaseContext context) : base(context)
        {
        }

        public User GetByCredentials(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                throw new BusinessException(ErrorCodes.BadRequest, "Usuário ou senha inválidos");
            return user;
        }

        public bool IsEmailAlreadyInUse(string email, long userId)
        {
            return _context.Users.Where(x => x.Email.Equals(email) && x.Id != userId).Count() > 0;
        }
    }
}
