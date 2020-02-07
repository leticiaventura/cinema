using System;
using System.Collections.ObjectModel;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Sessions;

namespace Cinema.Domain.Features.Movies
{
    public class Movie : Entity
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public int Length { get; set; }
        public byte[] Image { get; set; }
        public EnumAnimation Animation { get; set; }
        public EnumAudio Audio { get; set; }
        public Collection<Session> Sessions { get; set; }
    }
}
