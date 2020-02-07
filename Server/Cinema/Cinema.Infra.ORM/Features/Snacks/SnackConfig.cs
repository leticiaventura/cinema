using System.Data.Entity.ModelConfiguration;
using Cinema.Domain.Features.Snacks;

namespace Cinema.Infra.ORM.Features.Snacks
{
    public class MovieConfig : EntityTypeConfiguration<Snack>
    {
        public MovieConfig()
        {
            ToTable("Snacks");
            HasKey(u => u.Id);
            Property(u => u.Name).IsRequired();
            Property(u => u.Image).IsRequired();
            Property(u => u.Price).IsRequired();
        }
    }
}
