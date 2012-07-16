//using Com.Mobeelizer.Mobile.Wp7.Definition;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;
//using Com.Mobeelizer.Mobile.Wp7.Api;
//using System.Text;

//namespace wp7_sdk_unitTests.Tests.Definition
//{
//    [TestClass]
//    public class MobeelizerApplicationDefinitionTest
//    {
//        [TestMethod]
//        public void Digest()
//        {
//            MobeelizerApplicationDefinition definition = new MobeelizerApplicationDefinition();
//            definition.Application = "test2";
//            definition.ConflictMode = "OVERWRITE";
//            definition.Devices = new List<MobeelizerDeviceDefinition>();
//            definition.Groups = new List<MobeelizerGroupDefinition>();
//            definition.Models = new List<MobeelizerModelDefinition>();
//            definition.Roles = new List<MobeelizerRoleDefinition>();
//            definition.Vendor = "hajduczek";
//            MobeelizerModelDefinition modelDefinition = new MobeelizerModelDefinition();
//            modelDefinition.Name = "Blog";
//            modelDefinition.Credentials = new List<MobeelizerModelCredentialsDefinition>();
//            MobeelizerModelCredentialsDefinition modelCredentialDefiniton = new MobeelizerModelCredentialsDefinition();
//            modelCredentialDefiniton.CreateAllowed = MobeelizerCredential.ALL; // 3
//            modelCredentialDefiniton.DeleteAllowed = MobeelizerCredential.ALL; // 2
//            modelCredentialDefiniton.UpdateAllowed = MobeelizerCredential.ALL; // 0 
//            modelCredentialDefiniton.ReadAllowed = MobeelizerCredential.ALL; // 1
//            modelCredentialDefiniton.IsResolveConflictAllowed = true;
//            modelCredentialDefiniton.Role = "users-mobile";
//            modelDefinition.Credentials.Add(modelCredentialDefiniton);
//            modelDefinition.Fields = new List<MobeelizerModelFieldDefinition>();
//            MobeelizerModelFieldDefinition fieldDefiniton = new MobeelizerModelFieldDefinition();
//            fieldDefiniton.Credentials = new List<MobeelizerModelFieldCredentialsDefinition>();
//            MobeelizerModelFieldCredentialsDefinition credentialDefiniton = new MobeelizerModelFieldCredentialsDefinition();
//            credentialDefiniton.CreateAllowed = MobeelizerCredential.ALL; // 3
//            credentialDefiniton.UpdateAllowed = MobeelizerCredential.ALL; // 0 
//            credentialDefiniton.ReadAllowed = MobeelizerCredential.ALL; // 
//            credentialDefiniton.Role = "users-mobile";
//            fieldDefiniton.Credentials.Add(credentialDefiniton);
//            fieldDefiniton.Type = MobeelizerFieldType.TEXT;
//            fieldDefiniton.Name = "title";
//            fieldDefiniton.IsRequired = true;
//            modelDefinition.Fields.Add(fieldDefiniton);
//            MobeelizerModelFieldDefinition field2Definiton = new MobeelizerModelFieldDefinition();
//            field2Definiton.Credentials = new List<MobeelizerModelFieldCredentialsDefinition>();
//            MobeelizerModelFieldCredentialsDefinition credential2Definiton = new MobeelizerModelFieldCredentialsDefinition();
//            credential2Definiton.CreateAllowed = MobeelizerCredential.ALL; // 3
//            credential2Definiton.UpdateAllowed = MobeelizerCredential.ALL; // 0 
//            credential2Definiton.ReadAllowed = MobeelizerCredential.ALL; // 
//            credential2Definiton.Role = "users-mobile";
//            field2Definiton.Credentials.Add(credential2Definiton);
//            field2Definiton.Type = MobeelizerFieldType.TEXT;
//            field2Definiton.Name = "content";
//            field2Definiton.IsRequired = false;
//            modelDefinition.Fields.Add(field2Definiton);
//            definition.Models.Add(modelDefinition);
//            MobeelizerDeviceDefinition deviceDefinition = new MobeelizerDeviceDefinition() { Name = "mobile" };
//            definition.Devices.Add(deviceDefinition);
//            MobeelizerGroupDefinition groupDefinition = new MobeelizerGroupDefinition() { Name = "users" };
//            definition.Groups.Add(groupDefinition);
//            MobeelizerRoleDefinition roleDefinitions = new MobeelizerRoleDefinition() { Device = "mobile", Group = "users" };
//            definition.Roles.Add(roleDefinitions);
//            Assert.AreEqual(definition.Digest, "4685fc2e3f0ba8a3112f136cb8c5ad0be2f88464eee47a55fb0331cf0cc789e2");
//        }

//        [TestMethod]
//        public void DigestString()
//        {
//            MobeelizerApplicationDefinition definition = new MobeelizerApplicationDefinition();
//            definition.Application = "MobeelizerApp";
//            definition.ConflictMode = "OVERWRITE";
//            definition.Devices = new List<MobeelizerDeviceDefinition>();
//            definition.Groups = new List<MobeelizerGroupDefinition>();
//            definition.Models = new List<MobeelizerModelDefinition>();
//            definition.Roles = new List<MobeelizerRoleDefinition>();
//            definition.Vendor = "Mobeelizer";
//            MobeelizerModelDefinition modelDefinition = new MobeelizerModelDefinition();
//            modelDefinition.Name = "MobeelizerModel";
//            modelDefinition.Credentials = new List<MobeelizerModelCredentialsDefinition>();
//            MobeelizerModelCredentialsDefinition modelCredentialDefiniton = new MobeelizerModelCredentialsDefinition();
//            modelCredentialDefiniton.CreateAllowed = MobeelizerCredential.ALL; // 3
//            modelCredentialDefiniton.DeleteAllowed = MobeelizerCredential.GROUP; // 2
//            modelCredentialDefiniton.UpdateAllowed = MobeelizerCredential.NONE; // 0 
//            modelCredentialDefiniton.ReadAllowed = MobeelizerCredential.OWN; // 1
//            modelCredentialDefiniton.IsResolveConflictAllowed = true;
//            modelCredentialDefiniton.Role = "users-mobile";
//            modelDefinition.Credentials.Add(modelCredentialDefiniton);
//            modelDefinition.Fields = new List<MobeelizerModelFieldDefinition>();
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
//            modelDefinition.Fields.Add(fieldDefiniton);
//            definition.Models.Add(modelDefinition);
//            MobeelizerDeviceDefinition deviceDefinition = new MobeelizerDeviceDefinition() { Name = "mobile" };
//            definition.Devices.Add(deviceDefinition);
//            MobeelizerGroupDefinition groupDefinition = new MobeelizerGroupDefinition() { Name = "users" };
//            definition.Groups.Add(groupDefinition);
//            MobeelizerRoleDefinition roleDefinitions = new MobeelizerRoleDefinition() { Device = "mobile", Group = "users" };
//            definition.Roles.Add(roleDefinitions);
//            Assert.AreEqual(definition.DigestString, "Mobeelizer$MobeelizerApp$OVERWRITE$mobile$users${users$mobile}$MobeelizerModel{MobeelizerField{INTEGER$true$0$users-mobile=13000$}$users-mobile=13021}");
//        }

//        [TestMethod]
//        public void DigestSortJoinAndAdd()
//        {
//            IList<MobeelizerDeviceDefinition> devices = new List<MobeelizerDeviceDefinition>();
//            StringBuilder builder = new StringBuilder();
//            MobeelizerApplicationDefinition.DigestSortJoinAndAdd(builder, devices);
//            Assert.AreEqual(builder.ToString(), "");
//            devices.Add(new MobeelizerDeviceDefinition() { Name = "wp7" });
//            devices.Add(new MobeelizerDeviceDefinition() { Name = "android" });
//            devices.Add(new MobeelizerDeviceDefinition() { Name = "ios" });
//            MobeelizerApplicationDefinition.DigestSortJoinAndAdd(builder, devices);
//            Assert.AreEqual(builder.ToString(), "android&ios&wp7");
//        }
//    }
//}
