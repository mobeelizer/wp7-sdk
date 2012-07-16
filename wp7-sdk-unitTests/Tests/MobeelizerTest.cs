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
            MobeelizerLoginStatus loginStatus = MobeelizerLoginStatus.OTHER_FAILURE;
            Mobeelizer.Login("user", "password", (s) =>
                {
                    loginStatus = s;
                    loginEvent.Set();
                });
            loginEvent.WaitOne();
            Assert.AreEqual(loginStatus, MobeelizerLoginStatus.OK);
        }

        private ManualResetEvent login_02Event = new ManualResetEvent(false);

        [TestMethod]
        public void Login_02()
        {
            MobeelizerLoginStatus loginStatus = MobeelizerLoginStatus.OTHER_FAILURE;
            Mobeelizer.Login("user", "passsssword", (s) =>
                {
                    loginStatus = s;
                    login_02Event.Set();
                });
            login_02Event.WaitOne();
            Assert.AreEqual(loginStatus, MobeelizerLoginStatus.AUTHENTICATION_FAILURE);
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
                var departmentTable = db.GetModels<Department>();
                Department de = new Department();
                de.internalNumber = 1;
                de.name = "ddd";
                departmentTable.InsertOnSubmit(de);
                db.Commit();
                justAddEntityGuid = de.guid;
            }

            MobeelizerSyncStatus status = MobeelizerSyncStatus.NONE;
            Mobeelizer.SyncAll((s) =>
                {
                    status = s;
                    this.syncAllEvent.Set();
                });
            syncAllEvent.WaitOne();
            Assert.AreEqual(MobeelizerSyncStatus.FINISHED_WITH_SUCCESS, status);
            Department foundObject = null;

            using (IMobeelizerTransaction db = Mobeelizer.GetDatabase().BeginTransaction())
            {
                var departmentTable = db.GetModels<Department>();
                var query = from d in departmentTable where d.guid == justAddEntityGuid select d;
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
