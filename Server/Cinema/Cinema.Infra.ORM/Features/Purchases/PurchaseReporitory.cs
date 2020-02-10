using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Purchases.Interfaces;
using Cinema.Infra.ORM.Base;

namespace Cinema.Infra.ORM.Features.Purchases
{
    public class PurchaseRepository : AbstractRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(BaseContext context) : base(context)
        {
        }
    }
}
