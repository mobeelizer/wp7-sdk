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

        //public static MobeelizerLoginStatus Login(String instance, String login, String password)
        //{
        //    return Instance.Login(instance, login, password);
        //}

        public MobeelizerLoginStatus Login(String instance, String login, String password)
        {
            return Instance.Login(instance, login, password);
        }

        //public static MobeelizerLoginStatus Login(String login, String password)
        //{
        //    return Instance.Login(login, password);
        //}

        public static MobeelizerLoginStatus Login(String login, String password)
        {
            return Instance.Login(login, password);
        }

        public static void Logout()
        {
            Instance.Logout();
        }

        public static IMobeelizerDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        public static MobeelizerSyncStatus Sync()
        {
            return Instance.Sync();
        }

        public static MobeelizerSyncStatus SyncAll()
        {
            return Instance.SyncAll();
        }

        public static MobeelizerSyncStatus CheckSyncStatus()
        {
            return Instance.CheckSyncStatus();
        }

        // TODO: Notyfication
    }
}
