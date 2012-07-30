using Com.Mobeelizer.Mobile.Wp7;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace wp7_sdk_unitTests.Tests
{
    [TestClass]
    public class MobeelizerInternalDatabaseTest
    {
        //[TestMethod]
        //public void b_IsInitialSyncRequired()
        //{
        //    MobeelizerInternalDatabase database = new MobeelizerInternalDatabase();
        //    database.SetRoleAndInstanceGuid("mobeelizer", "user", "password", "role", "qqeqesdsfd13adssd");
        //    Assert.IsTrue(database.IsInitialSyncRequired("mobeelizer", "", "user"));
        //    //Assert.IsFalse(database.IsInitialSyncRequired("mobeelizer", "qqeqesdsfd13adssd", "user"));
        //    database.SetInitialSyncAsNotRequired("mobeelizer", "user");
        //    Assert.IsFalse(database.IsInitialSyncRequired("mobeelizer", "qqeqesdsfd13adssd", "user"));
        //    Assert.IsTrue(database.IsInitialSyncRequired("mobeelizer", "", "user"));
        //}

        //[TestMethod]
        //public void c_SetInitialSyncAsNotRequired()
        //{
        //    MobeelizerInternalDatabase database = new MobeelizerInternalDatabase();
        //    database.SetRoleAndInstanceGuid("mobeelizer", "user2", "password2", "role", "qqeqesdsfd13adssd");
        //  //  Assert.IsFalse(database.IsInitialSyncRequired("mobeelizer", "qqeqesdsfd13adssd", "user2"));
        //    database.SetInitialSyncAsNotRequired("mobeelizer", "user2");
        //    Assert.IsFalse(database.IsInitialSyncRequired("mobeelizer", "qqeqesdsfd13adssd", "user2"));
        //}

        //[TestMethod]
        //public void a_SetRoleAndInstanceGuid()
        //{
        //    MobeelizerInternalDatabase database = new MobeelizerInternalDatabase();
        //    database.SetRoleAndInstanceGuid("mobeelizer", "user3", "password", "role3", "qqeqesdsfd13adssd");
        //    Assert.AreEqual("qqeqesdsfd13adssd", database.GetRoleAndInstanceGuid("mobeelizer", "user3", "password")[1]);
        //    Assert.AreEqual("role3", database.GetRoleAndInstanceGuid("mobeelizer", "user3", "password")[0]);
        //}

        //[TestMethod]
        //public void d_ClearRoleAndInstanceGuid()
        //{
        //    MobeelizerInternalDatabase database = new MobeelizerInternalDatabase();
        //    database.SetRoleAndInstanceGuid("mobeelizer", "user4", "password", "role4", "qqeqesdsfd13adssd");
        //    database.ClearRoleAndInstanceGuid("mobeelizer", "user4");
        //    Assert.AreEqual(database.GetRoleAndInstanceGuid("mobeelizer", "user4", "password")[1], null);
        //    Assert.AreEqual(database.GetRoleAndInstanceGuid("mobeelizer", "user4", "password")[0], null);
        //}

        //[TestMethod]
        //public void b_GetRoleAndInstanceGuid()
        //{
        //    MobeelizerInternalDatabase database = new MobeelizerInternalDatabase();
        //    database.SetRoleAndInstanceGuid("mobeelizer", "user5", "password", "role5", "qqeqesdsfd13adssd");
        //    Assert.AreEqual("qqeqesdsfd13adssd", database.GetRoleAndInstanceGuid("mobeelizer", "user5", "password")[1]);
        //    Assert.AreEqual("role5", database.GetRoleAndInstanceGuid("mobeelizer", "user5", "password")[0]);
        //    Assert.AreEqual(database.GetRoleAndInstanceGuid("mobeelizer", "user5", "wrongpassword")[1], null);
        //    Assert.AreEqual(database.GetRoleAndInstanceGuid("mobeelizer", "user5", "wrongpassword")[0], null);
        //}
    }
}
