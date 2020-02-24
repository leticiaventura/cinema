using System.Data.Entity.ModelConfiguration;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Users;

namespace Cinema.Infra.ORM.Features.Purchases
{
    public class PurchaseConfig : EntityTypeConfiguration<Purchase>
    {
        public PurchaseConfig()
        {
            ToTable("Purchases");
            HasKey(u => u.Id);
            Property(u => u.MovieName).IsRequired();
            Property(u => u.Total).IsRequired();
            Property(u => u.XmlSnacks).IsRequired();
            Property(u => u.Date).IsRequired();
            Property(u => u.SessionDate).IsRequired();
            HasRequired<Session>(s => s.Session).WithMany(m => m.Purchases).HasForeignKey(s => s.SessionId);
            HasRequired<User>(s => s.User).WithMany(m => m.Purchases).HasForeignKey(s => s.UserId);
            Ignore(u => u.SnacksArray);
        }
    }
}
