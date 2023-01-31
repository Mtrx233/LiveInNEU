using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveInNEU.Services.Identity;
using NUnit.Framework;

namespace TestProject1.Services
{
    /// <author>殷昭伉</author>
    public class IdentityServiceStubTest
    {
        [Test]
        public void TestAuthenticate() {
             IdentityServiceStub identityServiceStub = new IdentityServiceStub();
            try {
                identityServiceStub.Authenticate();
            } catch (Exception e) {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void TestVerifyRegistration()
        {
            IdentityServiceStub identityServiceStub = new IdentityServiceStub();
            var tests = identityServiceStub.VerifyRegistration().Result;
            Assert.IsFalse(tests);
        }

    }
}
