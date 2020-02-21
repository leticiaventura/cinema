using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Purchases
{
    public class Seat : Entity
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
