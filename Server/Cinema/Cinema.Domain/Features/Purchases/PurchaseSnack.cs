using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Domain.Features.Snacks;

namespace Cinema.Domain.Features.Purchases
{
    public class PurchaseSnack : Snack
    {
        public int Quantity { get; set; }
    }
}
