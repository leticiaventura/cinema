using System.Data.Entity.ModelConfiguration;
using Cinema.Domain.Features.Movies;

namespace Cinema.Infra.ORM.Features.Movies
{
    public class MovieConfig : EntityTypeConfiguration<Movie>
    {
        public MovieConfig()
        {
            ToTable("Movies");
            HasKey(u => u.Id);
            Property(u => u.Name).IsRequired();
            Property(u => u.Description).IsRequired();
            Property(u => u.Image).IsRequired();
            Property(u => u.Length).IsRequired();
            Property(u => u.Animation).IsRequired();
            Property(u => u.Audio).IsRequired();
        }
    }
}
