namespace Cinema.Application.Features.Users.ViewModels
{
    public class UserViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual int PermissionLevel { get; set; }
    }
}
