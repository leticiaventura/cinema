using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Purchases.Interfaces;
using Cinema.Infra.ORM.Base;
using System.Data.Entity;

namespace Cinema.Infra.ORM.Features.Purchases
{
    public class PurchaseRepository : AbstractRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(BaseContext context) : base(context)
        {
        }
        public override IQueryable<Purchase> GetAll()
        {
            return _context.Purchases.Include(s => s.Session).Include(s => s.User);
        }
    }
}
