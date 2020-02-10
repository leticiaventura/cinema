using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Users;

namespace Cinema.Domain.Features.Purchases
{
    public class Purchase : Entity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public double Total { get; set; }
        public int Seats { get; set; }
        public DateTime Date { get; set; }
        public long SessionId { get; set; }
        public Session Session { get; set; }
        public string MovieName { get; set; }
        public List<PurchaseSnack> SnacksArray { get; set; }
        public string XmlSnacks { get => SerializeSnack(); set { } }

        private string SerializeSnack()
        {
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(List<PurchaseSnack>));
                serializer.Serialize(stringwriter, SnacksArray);
                return stringwriter.ToString();
            }
        }

        public double CalculateTotal()
        {
            double sum = 0;
            foreach (var snack in SnacksArray)
            {
                sum += snack.Price * snack.Quantity;
            }
            sum += Session.Price * Seats;

            return sum;
        }
    }
}
