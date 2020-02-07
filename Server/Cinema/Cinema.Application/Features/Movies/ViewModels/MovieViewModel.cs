namespace Cinema.Application.Features.Movies.ViewModels
{
    public class MovieViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Image { get; set; }
        public virtual int Length { get; set; }
        public virtual int Animation { get; set; }
        public virtual int Audio { get; set; }
    }
}
