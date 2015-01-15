using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace AsyncPluggableProtocol.Tests
{
    [TestFixture]
    public class ProtocolFactoryTests
    {
        [Test]
        public void ProtocolFactory_Object_ImplementsIClassFactory()
        {
            // Arrange & Act
            var protocolFactory = new ProtocolFactory(null);

            // Assert
            Assert.IsInstanceOf<IClassFactory>(protocolFactory, "ProtocolFactory must implement IClassFactory interface.");
        }
    }
}
