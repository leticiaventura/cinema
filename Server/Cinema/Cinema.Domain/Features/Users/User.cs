using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Purchases;

namespace Cinema.Domain.Features.Users
{
    public class User : Entity
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public EnumPermissionLevel Permission { get; set; }
        public Collection<Purchase> Purchases { get; set; }
    }
}
