//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Com.Mobeelizer.Mobile.Wp7.Definition;
//using System.Collections.Generic;
//using Com.Mobeelizer.Mobile.Wp7.Api;

//namespace wp7_sdk_unitTests.Tests.Definition
//{
//    [TestClass]
//    public class MobeelizerModelDefinitionTest
//    {
//        [TestMethod]
//        public void TypeTest()
//        {
//            MobeelizerModelDefinition definiton = new MobeelizerModelDefinition();
//            Assert.IsInstanceOfType(definiton, typeof(IMobeelizerDefinition));
//        }

//        [TestMethod]
//        public void Digest()
//        {
//            MobeelizerModelDefinition definiton = new MobeelizerModelDefinition();
//            definiton.Name = "MobeelizerModel";
//            definiton.Credentials = new List<MobeelizerModelCredentialsDefinition>();
//            MobeelizerModelCredentialsDefinition modelCredentialDefiniton = new MobeelizerModelCredentialsDefinition();
//            modelCredentialDefiniton.CreateAllowed = MobeelizerCredential.ALL; // 3
//            modelCredentialDefiniton.DeleteAllowed = MobeelizerCredential.GROUP; // 2
//            modelCredentialDefiniton.UpdateAllowed = MobeelizerCredential.NONE; // 0 
//            modelCredentialDefiniton.ReadAllowed = MobeelizerCredential.OWN; // 1
//            modelCredentialDefiniton.IsResolveConflictAllowed = true;
//            modelCredentialDefiniton.Role = "users-mobile";
//            definiton.Credentials.Add(modelCredentialDefiniton);
//            definiton.Fields = new List<MobeelizerModelFieldDefinition>();
//            MobeelizerModelFieldDefinition fieldDefiniton = new MobeelizerModelFieldDefinition();
//            fieldDefiniton.Credentials = new List<MobeelizerModelFieldCredentialsDefinition>();
//            MobeelizerModelFieldCredentialsDefinition credentialDefiniton = new MobeelizerModelFieldCredentialsDefinition();
//            credentialDefiniton.CreateAllowed = MobeelizerCredential.ALL; // 3
//            credentialDefiniton.UpdateAllowed = MobeelizerCredential.NONE; // 0 
//            credentialDefiniton.ReadAllowed = MobeelizerCredential.OWN; // 
//            credentialDefiniton.Role = "users-mobile";
//            fieldDefiniton.Credentials.Add(credentialDefiniton);
//            fieldDefiniton.DefaultValue = "0";
//            fieldDefiniton.Type = MobeelizerFieldType.INTEGER;
//            fieldDefiniton.Name = "MobeelizerField";
//            fieldDefiniton.IsRequired = true;
//            definiton.Fields.Add(fieldDefiniton);
//            Assert.AreEqual(definiton.DigestString, "MobeelizerModel{MobeelizerField{INTEGER$true$0$users-mobile=13000$}$users-mobile=13021}");
//        }
//    }
//}
