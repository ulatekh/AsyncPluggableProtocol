using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FakeItEasy;

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

        [Test]
        public void GetSession__CoInternetGetSession_ReturnsOkAndInternetSessionObject__ReturnsInternetSessionObject()
        {
            // Arrange
            var expectedInternetSessionObject = A.Fake<IInternetSession>();
            var nativeStub = A.Fake<INativeMethods>();

            IInternetSession ignoredInternetSession;
            A.CallTo(() => nativeStub.CoInternetGetSession(A<int>.Ignored, out ignoredInternetSession, A<int>.Ignored))
                .Returns(NativeConstants.S_OK)
                .AssignsOutAndRefParameters(expectedInternetSessionObject);

            // Act
            IInternetSession actualInternetSessionObject = ProtocolFactory.GetSession(nativeStub);

            // Assert
            Assert.AreSame(expectedInternetSessionObject, actualInternetSessionObject, "ProtocolFactory.GetSession() must return the object returned from CoInternetGetSession()");
        }
    }
}
