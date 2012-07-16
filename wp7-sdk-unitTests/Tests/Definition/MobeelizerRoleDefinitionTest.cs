//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Com.Mobeelizer.Mobile.Wp7.Definition;

//namespace wp7_sdk_unitTests.Tests.Definition
//{
//    [TestClass]
//    public class MobeelizerRoleDefinitionTest
//    {
//        [TestMethod]
//        public void TypeTest()
//        {
//            MobeelizerRoleDefinition definiton = new MobeelizerRoleDefinition();
//            Assert.IsInstanceOfType(definiton, typeof(IMobeelizerDefinition));
//        }

//        [TestMethod]
//        public void Digest()
//        {
//            MobeelizerRoleDefinition definiton = new MobeelizerRoleDefinition();
//            definiton.Device = "mobile";
//            definiton.Group = "users";
//            Assert.AreEqual(definiton.DigestString, "{users$mobile}");
//        }

//        [TestMethod]
//        public void ResolveName()
//        {
//            MobeelizerRoleDefinition definiton = new MobeelizerRoleDefinition();
//            definiton.Device = "mobile";
//            definiton.Group = "users";
//            Assert.AreEqual(definiton.ResolveName(), "users-mobile");
//        }
//    }
//}
