using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Infra.ORM.Base;
using System.Data.Common;


namespace Cinema.Infra.ORM.Tests.Context
{
    public class FakeDbContext : BaseContext
    {
        public FakeDbContext(DbConnection connection) : base(connection)
        {

        }
    }
}
