using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinema.Application.Mapping;
using NUnit.Framework;

namespace Cinema.Application.Tests.Initializer
{
    [TestFixture]
    public class TestServiceBase
    {
        protected Mapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mapper = new AutoMapperInitializer().GetMapper();
        }
    }
}
