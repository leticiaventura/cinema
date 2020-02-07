using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Users.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByCredentials(string email, string password);
        bool IsEmailAlreadyInUse(string email, long userId);

    }
}
