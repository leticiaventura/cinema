using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Users.Interfaces
{
    public interface IUserService : IService<User>
    {
        User Login(string email, string password);

        bool IsEmailAlreadyInUse(string email, long userId);
    }
}
