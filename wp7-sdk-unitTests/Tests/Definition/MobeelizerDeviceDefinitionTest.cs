using Com.Mobeelizer.Mobile.Wp7.Definition;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace wp7_sdk_unitTests.Tests.Definition
{
    [TestClass]
    public class MobeelizerDeviceDefinitionTest
    {
        [TestMethod]
        public void TypeTest()
        {
            MobeelizerDeviceDefinition definiton = new MobeelizerDeviceDefinition();
            Assert.IsInstanceOfType(definiton, typeof(IMobeelizerDefinition));
        }

        [TestMethod]
        public void Digest()
        {
            MobeelizerDeviceDefinition definiton = new MobeelizerDeviceDefinition();
            definiton.Name = "MobeelizerTestCase1";
            Assert.AreEqual(definiton.DigestString, definiton.Name);
        }
    }
}
