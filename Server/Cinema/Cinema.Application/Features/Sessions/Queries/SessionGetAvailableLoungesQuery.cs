using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.Features.Sessions.Queries
{
    public class SessionGetAvailableLoungesQuery
    {
        public virtual DateTime Start { get; set; }
        public virtual int MovieLength { get; set; }
    }
}
