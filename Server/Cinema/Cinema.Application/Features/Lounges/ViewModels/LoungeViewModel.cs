namespace Cinema.Application.Features.Lounges.ViewModels
{
    public class LoungeViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Rows { get; set; }
        public virtual int Columns { get; set; }
    }
}
