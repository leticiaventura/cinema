using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Features.Users
{
    public enum EnumPermissionLevel
    {
        None = 0,
        Admin = 1,
        Employee = 2,
        Customer = 3
    }
}