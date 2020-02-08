using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Domain.Common
{
    public class Entity
    {
        public virtual long Id { get; set; }

        public virtual void Validate()
        {
            
        }
    }
}
