using System.Data.Entity.ModelConfiguration;
using Cinema.Domain.Features.Users;

namespace Cinema.Infra.ORM.Features.Users
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            ToTable("Users");
            HasKey(u => u.Id);
            Property(u => u.Name).IsRequired();
            Property(u => u.Password).IsRequired();
            Property(u => u.Email).IsRequired();
            Property(u => u.Permission).IsRequired();
        }
    }
}
