using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Com.Mobeelizer.Mobile.Wp7;
using System.Threading;
using Com.Mobeelizer.Mobile.Wp7.Api;
using wp7_sdk_unitTests.Models;
using System.Linq;
using wp7_sdk_unitTests.Helpers.Mock;

namespace wp7_sdk_unitTests.Tests
{
    [TestClass]
    public class MobeelizerTest
    {
        [TestMethod]
        public void _OnLaunching()
        {
            Mobeelizer.OnLaunching();
        }

        private ManualResetEvent loginEvent = new ManualResetEvent(false);
    
        [TestMethod]
        public void Login()
        {
            UTWebRequest.SyncData = "firstSync.zip";
            MobeelizerOperationError loginStatus = null;
            Mobeelizer.Login("user", "password", (s) =>
                {
                    loginStatus = s;
                    loginEvent.Set();
                });
            loginEvent.WaitOne();
            Assert.IsNull(loginStatus);
        }

        private ManualResetEvent login_02Event = new ManualResetEvent(false);

        [TestMethod]
        public void Login_02()
        {
            MobeelizerOperationError loginStatus = null;
            Mobeelizer.Login("user", "passsssword", (s) =>
                {
                    loginStatus = s;
                    login_02Event.Set();
                });
            login_02Event.WaitOne();
            Assert.IsNotNull(loginStatus);
        }

        private ManualResetEvent syncAllLoginEvent = new ManualResetEvent(false);
        private ManualResetEvent syncAllEvent = new ManualResetEvent(false);

        [TestMethod]
        public void SyncAll()
        {
            UTWebRequest.SyncData = "firstSync.zip";
            Mobeelizer.Login("user", "password", (s) =>
            {
                syncAllLoginEvent.Set();
            });
            syncAllLoginEvent.WaitOne();
            String justAddEntityGuid = string.Empty;
            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departmentTable = db.GetModelSet<Department>();
                Department de = new Department();
                de.InternalNumber = 1;
                de.Name = "ddd";
                departmentTable.InsertOnSubmit(de);
                db.SubmitChanges();
                justAddEntityGuid = de.Guid;
            }

            MobeelizerOperationError status = null;
            Mobeelizer.SyncAll((s) =>
                {
                    status = s;
                    this.syncAllEvent.Set();
                });
            syncAllEvent.WaitOne();
            Assert.IsNull(status);
            Department foundObject = null;

            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departmentTable = db.GetModelSet<Department>();
                var query = from d in departmentTable where d.Guid == justAddEntityGuid select d;
                try
                {
                    foundObject = query.Single();
                }
                catch { }
                Assert.IsNull(foundObject);
                Assert.AreEqual(1, departmentTable.Count());
            }
        }
    }
}
