using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Application.Features.Movies.ViewModels
{
    public class MovieGridViewModel
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Length { get; set; }
        public virtual int Animation { get; set; }
        public virtual int Audio { get; set; }
    }
}
