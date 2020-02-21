using System.Data.Entity.ModelConfiguration;
using Cinema.Domain.Features.Lounges;

namespace Cinema.Infra.ORM.Features.Lounges
{
    public class LoungeConfig : EntityTypeConfiguration<Lounge>
    {
        public LoungeConfig()
        {
            ToTable("Lounges");
            HasKey(u => u.Id);
            Property(u => u.Name).IsRequired();
            Property(u => u.Rows).IsRequired();
            Property(u => u.Columns).IsRequired();
        }
    }
}
