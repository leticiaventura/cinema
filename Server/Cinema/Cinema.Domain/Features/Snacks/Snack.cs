using System;
using Cinema.Domain.Common;

namespace Cinema.Domain.Features.Snacks
{
    public class Snack : Entity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public byte[] Image { get; set; }
    }
}
