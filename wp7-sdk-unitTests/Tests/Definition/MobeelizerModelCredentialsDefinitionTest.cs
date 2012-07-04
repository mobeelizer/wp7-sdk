using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace wp7_sdk_unitTests.Tests.Definition
{
    [TestClass]
    public class MobeelizerModelCredentialsDefinitionTest
    {
        [TestMethod]
        public void TypeTest()
        {
            MobeelizerModelCredentialsDefinition definiton = new MobeelizerModelCredentialsDefinition();
            Assert.IsInstanceOfType(definiton, typeof(IMobeelizerDefinition));
        }

        [TestMethod]
        public void Digest()
        {
            MobeelizerModelCredentialsDefinition definiton = new MobeelizerModelCredentialsDefinition();
            definiton.CreateAllowed = MobeelizerCredential.ALL; // 3
            definiton.DeleteAllowed = MobeelizerCredential.GROUP; // 2
            definiton.UpdateAllowed = MobeelizerCredential.NONE; // 0 
            definiton.ReadAllowed = MobeelizerCredential.OWN; // 1
            definiton.IsResolveConflictAllowed = true;
            definiton.Role = "users-mobile";
            Assert.AreEqual(definiton.DigestString, "users-mobile=13021");
        }
    }
}
