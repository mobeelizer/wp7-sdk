//using System.Xml.Linq;
//using Com.Mobeelizer.Mobile.Wp7.Definition;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace wp7_sdk_unitTests.Tests.Definition
//{
//    [TestClass]
//    public class MobeelizerDefinitionParserTest
//    {
//        [TestMethod]
//        public void Parse01()
//        {
//            XDocument document = XDocument.Load("Resources/Definitions/definition_1.xml");
//            MobeelizerApplicationDefinition definition = MobeelizerDefinitionParser.Parse(document);
//            Assert.AreEqual(definition.Digest, "4685fc2e3f0ba8a3112f136cb8c5ad0be2f88464eee47a55fb0331cf0cc789e2");
//        }

//        [TestMethod]
//        public void Parse02()
//        {
//            XDocument document = XDocument.Load("Resources/Definitions/definition_2.xml");
//            MobeelizerApplicationDefinition definition = MobeelizerDefinitionParser.Parse(document);
//            Assert.AreEqual(definition.Digest, "3203e926bd4885307b5ef29e2646983c37aefc56f7ab6579624f5223eb482f60");
//        }

//        [TestMethod]
//        public void Parse03()
//        {
//            XDocument document = XDocument.Load("Resources/Definitions/definition_3.xml");
//            MobeelizerApplicationDefinition definition = MobeelizerDefinitionParser.Parse(document);
//            Assert.AreEqual(definition.Digest, "aa7fa806f75192575d332d12db51d53504364ee65f6ae312dc633226c918f2d8");
//        }
//    }
//}
