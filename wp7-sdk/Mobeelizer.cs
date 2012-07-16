using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Configuration;
using Microsoft.Practices.Mobile.Configuration;
using System.Reflection;

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

        // TODO: Notyfication
    }
}
