using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_sdk_unitTests.Tests.Definition
{
    [TestClass]
    public class MobeelizerModelFieldDefinitionTest
    {
        
        [TestMethod]
        public void TypeTest()
        {
            MobeelizerModelFieldDefinition definiton = new MobeelizerModelFieldDefinition();
            Assert.IsInstanceOfType(definiton, typeof(IMobeelizerDefinition));
        }

        [TestMethod]
        public void Digest()
        {
            MobeelizerModelFieldDefinition definiton = new MobeelizerModelFieldDefinition();
            definiton.Credentials = new List<MobeelizerModelFieldCredentialsDefinition>();
            MobeelizerModelFieldCredentialsDefinition credentialDefiniton = new MobeelizerModelFieldCredentialsDefinition();
            credentialDefiniton.CreateAllowed = MobeelizerCredential.ALL; // 3
            credentialDefiniton.UpdateAllowed = MobeelizerCredential.NONE; // 0 
            credentialDefiniton.ReadAllowed = MobeelizerCredential.OWN; // 
            credentialDefiniton.Role = "users-mobile";
            definiton.Credentials.Add(credentialDefiniton);
            definiton.DefaultValue = "0";
            definiton.Type = MobeelizerFieldType.INTEGER;
            definiton.Name = "MobeelizerField";
            definiton.IsRequired = true;
            Assert.AreEqual(definiton.DigestString, "MobeelizerField{INTEGER$true$0$users-mobile=13000$}");
        }

        [TestMethod]
        public void Digest_Options()
        {
            // TODO: test it with options.
        }
    }
}
