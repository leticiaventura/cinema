using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Effort.Provider;
using NUnit.Framework;

namespace Cinema.Infra.ORM.Tests.Initializer
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class EffortTestBase
    {
        [OneTimeSetUp]
        public void OneTimeInitializer()
        {
            EffortProviderConfiguration.RegisterProvider();
        }

        [SetUp]
        public void Initialize()
        {
            EffortProviderFactory.ResetDb();
        }
    }
}
