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
            Mobeelizer.Login("user", "password", (status) => 
            {
                loginStatus = status;
         //       resetEvent.Set();
            });
        //    resetEvent.WaitOne();
          //  Assert.AreEqual(loginStatus, MobeelizerLoginStatus.OK);
        }

        ManualResetEvent allDone = new ManualResetEvent(false);

        //[TestMethod]
        public void Login_02()
        {
            MobeelizerLoginStatus loginStatus = MobeelizerLoginStatus.OTHER_FAILURE;
           // ManualResetEvent resetEvent = new ManualResetEvent(false);
            Mobeelizer.Login("user", "wrongpassword", (status) =>
            {
                loginStatus = status;
                allDone.Set();
            });
      //      allDone.WaitOne();
      //      Assert.AreEqual(loginStatus, MobeelizerLoginStatus.AUTHENTICATION_FAILURE);
        }

        [TestMethod]
        public void SyncAll()
        {
            Mobeelizer.Login("user", "password", (s) =>
                {
                    Mobeelizer.SyncAll((MobeelizerSyncStatus status) =>
                        {

                        });
                });
        }
    }
}
