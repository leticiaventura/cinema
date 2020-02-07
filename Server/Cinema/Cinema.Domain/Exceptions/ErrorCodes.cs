using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Exceptions
{
    public enum ErrorCodes
    {
        BadRequest = 400,

        NotFound = 404,

        Unhandled = 500
    }
}
