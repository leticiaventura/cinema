using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.Features.Purchases.ViewModels
{
    public class PurchasedTicketGridViewModel
    {
        public long Id { get; set; }
        public string Movie { get; set; }
        public string Date { get; set; }
        public string Lounge { get; set; }
        public string Email { get; set; }
    }
}
