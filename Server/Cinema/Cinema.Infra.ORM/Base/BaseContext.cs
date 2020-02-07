using System.Data.Entity;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Users;

namespace Cinema.Infra.ORM.Base
{
    public class BaseContext : DbContext
    {
        public BaseContext() : base("DBCinema")
        {
            Database.SetInitializer(new DbInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lounge> Lounges { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Snack> Snacks { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
