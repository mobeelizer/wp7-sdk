using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Configuration;
using Microsoft.Practices.Mobile.Configuration;
using System.Reflection;
using Com.Mobeelizer.Mobile.Wp7.Database;
using System.IO;
using System.Collections.Generic;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class Mobeelizer
    {
        private static MobeelizerApplication instance;

        internal static MobeelizerApplication Instance
        {
            get
            {
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        public static String VERSION
        {
            get
            {
                //TODO 
                //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return "1.0.0";
            }
        }

        public static bool IsLoggedIn
        {
            get
            {
                return Instance.IsLoggedIn;
            }
        }

        public static void OnLaunching()
        {
            Instance = MobeelizerApplication.CreateApplication();
            Instance.GetTombstoningManager().ClearAndUnlockSavedState();
        }

        public static void OnActivated(bool isApplicationInstancePreserved)
        {
            if (!isApplicationInstancePreserved)
            {
                Instance = MobeelizerApplication.CreateApplication();
                Instance.GetTombstoningManager().RestoreApplicationState();
            }

            Instance.GetTombstoningManager().ClearSavedState();
        }

        public static void OnDeactivated()
        {
            Instance.GetTombstoningManager().SaveApplicationState();
        }

        public static void OnClosing()
        {
            Instance.Logout();
        }

        public void Login(String instance, String login, String password, MobeelizerLoginCallback callback)
        {
            Instance.Login(instance, login, password, callback);
        }

        public static void Login(String login, String password, MobeelizerLoginCallback callback)
        {
            Instance.Login(login, password, callback);
        }

        public static void Logout()
        {
            Instance.Logout();
        }

        public static IMobeelizerDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        public static void Sync(MobeelizerSyncCallback callback)
        {
            Instance.Sync(callback);
        }

        public static void SyncAll(MobeelizerSyncCallback callback)
        {
            Instance.SyncAll(callback);
        }

        public static MobeelizerSyncStatus CheckSyncStatus()
        {
            return Instance.CheckSyncStatus();
        }

        public static IMobeelizerFile CreateFile(String name, Stream stream)
        {
            return new MobeelizerFile(name, stream);
        }

        public static IMobeelizerFile CreateFile(String name, String guid)
        {
            return new MobeelizerFile(name, guid);
        }

        public static void RegisterForRemoteNotifications(String chanelUri, MobeelizerNotificationCallback callback)
        {
            Instance.RegisterForRemoteNotifications(chanelUri, callback);
        }

        public static void UnregisterForRemoteNotifications(MobeelizerNotificationCallback callback)
        {
            Instance.UnregisterForRemoteNotifications(callback);
        }

        public static void SendRemoteNotification(IDictionary<String, String> notification, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(null, null, null, notification, callback);
        }

        public static void  SendRemoteNotificationToDevice(IDictionary<String, String> notification, String device, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(device, null, null, notification, callback);
        }

        public static void  SendRemoteNotificationToUsers(IDictionary<String, String> notification, List<String> users, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(null, null, users, notification, callback);
        }

        public static void SendRemoteNotificationToUsersOnDevice(IDictionary<String, String> notification, List<String> users, String device, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(device, null, users, notification, callback);
        }

        public static void SendRemoteNotificationToGroup(IDictionary<String, String> notification, String group, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(null, group, null, notification, callback);
        }

        public static void SendRemoteNotificationToGroupOnDevice(IDictionary<String, String> notification, String group, String device, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(device, group, null, notification, callback);
        }
    }
}
