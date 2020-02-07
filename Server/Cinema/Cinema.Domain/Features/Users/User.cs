using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Users
{
    public class User : Entity
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public EnumPermissionLevel Permission { get; set; }
    }
}
