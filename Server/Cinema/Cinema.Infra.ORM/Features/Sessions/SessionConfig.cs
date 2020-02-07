using System.Data.Entity.ModelConfiguration;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Sessions;

namespace Cinema.Infra.ORM.Features.Sessions
{
    public class SessionConfig : EntityTypeConfiguration<Session>
    {
        public SessionConfig()
        {
            ToTable("Sessions");
            HasKey(u => u.Id);
            Property(u => u.Price).IsRequired();
            Property(u => u.Start).IsRequired();
            Property(u => u.End).IsRequired();
            HasRequired<Movie>(s => s.Movie).WithMany(m => m.Sessions).HasForeignKey(s => s.MovieId);
            HasRequired<Lounge>(s => s.Lounge).WithMany(m => m.Sessions).HasForeignKey(s => s.LoungeId);
        }
    }
}
