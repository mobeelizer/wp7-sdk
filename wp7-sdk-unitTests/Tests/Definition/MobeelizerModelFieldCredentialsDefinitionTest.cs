using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace wp7_sdk_unitTests.Tests.Definition
{
    [TestClass]
    public class MobeelizerModelFieldCredentialsDefinitionTest
    {
        [TestMethod]
        public void TypeTest()
        {
            MobeelizerModelFieldCredentialsDefinition definiton = new MobeelizerModelFieldCredentialsDefinition();
            Assert.IsInstanceOfType(definiton, typeof(IMobeelizerDefinition));
        }

        [TestMethod]
        public void Digest()
        {
            MobeelizerModelFieldCredentialsDefinition definiton = new MobeelizerModelFieldCredentialsDefinition();
            definiton.CreateAllowed = MobeelizerCredential.ALL; // 3
            definiton.UpdateAllowed = MobeelizerCredential.NONE; // 0 
            definiton.ReadAllowed = MobeelizerCredential.OWN; // 
            definiton.Role = "users-mobile";
            Assert.AreEqual(definiton.DigestString, "users-mobile=13000");
        }
    }
}
