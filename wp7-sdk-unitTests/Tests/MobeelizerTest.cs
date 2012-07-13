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
        //ManualResetEvent resetEvent = new ManualResetEvent(false);
            
      //  [TestMethod]
        public void Login()
        {
            MobeelizerLoginStatus loginStatus = MobeelizerLoginStatus.OTHER_FAILURE;
            loginStatus  = Mobeelizer.Login("user", "password");
            Assert.AreEqual(loginStatus, MobeelizerLoginStatus.OK);
        }

        ManualResetEvent allDone = new ManualResetEvent(false);

        //[TestMethod]
        public void Login_02()
        {
            MobeelizerLoginStatus loginStatus = MobeelizerLoginStatus.OTHER_FAILURE;
            loginStatus = Mobeelizer.Login("user", "passsssword");
            Assert.AreEqual(loginStatus, MobeelizerLoginStatus.AUTHENTICATION_FAILURE);
        }

        [TestMethod]
        public void SyncAll()
        {
            Mobeelizer.Login("user", "password");
            IMobeelizerDatabase db = Mobeelizer.GetDatabase();
            var table = db.GetModels<Department>();

            //Department de = new Department();
            //de.internalNumber = 1;
            //de.name = "ddd";

            //table.InsertOnSubmit(de);
            //db.Commit();

            var result = from departement in db.GetModels<Department>() select departement;
            foreach (Department dep in result)
            {

            }

            MobeelizerSyncStatus status = Mobeelizer.SyncAll();
        }
    }
}
