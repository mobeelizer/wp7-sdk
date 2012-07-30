using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace Com.Mobeelizer.Mobile.Wp7
{
    /// <summary>
    /// Entry point to the Mobeelizer application that holds references to the user sessions and the database.
    /// <code>
    /// // login
    ///     Mobeelizer.Login("user", "password", CallbackMethod);
    /// // get database 
    ///     Mobeelizer.GetDatabase();
    /// // logout 
    ///     Mobeelizer.Logout();
    /// </code>
    /// </summary>
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

        /// <summary>
        /// Listener used to notify about synchronization status change.
        /// </summary>
        public static event MobeelizerSyncStatusChangedEventHandler SyncStatusChanged;

        /// <summary>
        /// Version of Mobeelizer SDK.
        /// </summary>
        public static String VERSION
        {
            get
            {
                var assemblyName = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
                return assemblyName.Version.ToString();
            }
        }

        /// <summary>
        /// Check if the user session is active.
        /// </summary>
        /// <value>
        /// True if user session is active.
        /// </value>
        public static bool IsLoggedIn
        {
            get
            {
                return Instance.IsLoggedIn;
            }
        }

        /// <summary>
        /// Initialize mobeelizer application. Execute this method in your App.xaml.cs when application is lounching.
        /// </summary>
        public static void OnLaunching()
        {
            Instance = MobeelizerApplication.CreateApplication();
            Instance.SyncStatusChanged += OnSyncStatusChanged;
            Instance.GetTombstoningManager().ClearAndUnlockSavedState();
        }

        /// <summary>
        /// Restore mobeelizer applicaion state after tombstoning. 
        /// Execute this method in your App.xaml.cs when application is activated.
        /// </summary>
        /// <param name="isApplicationInstancePreserved">Indicates whether yuours application instance is preserved.</param>
        public static void OnActivated(bool isApplicationInstancePreserved)
        {
            if (!isApplicationInstancePreserved)
            {
                Instance = MobeelizerApplication.CreateApplication();
                Instance.SyncStatusChanged += OnSyncStatusChanged;
                Instance.GetTombstoningManager().RestoreApplicationState();
            }

            Instance.GetTombstoningManager().ClearSavedState();
        }

        /// <summary>
        /// Saves mobeelizer application state before tombstoning.
        /// Execute this method in your App.xaml.cs when application is deactivated.
        /// </summary>
        public static void OnDeactivated()
        {
            Instance.GetTombstoningManager().SaveApplicationState();
        }

        /// <summary>
        /// Closes user session when yours application is closing.
        /// </summary>
        public static void OnClosing()
        {
            Instance.SyncStatusChanged -= OnSyncStatusChanged;
            Instance.Logout();
        }

        /// <summary>
        /// Create a user session for the given login, password and instance.
        /// </summary>
        /// <param name="instance">Instance's name.</param>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <param name="callback">Callback method.</param>
        public void Login(String instance, String login, String password, MobeelizerLoginCallback callback)
        {
            Instance.Login(instance, login, password, callback);
        }

        /// <summary>
        /// Create a user session for the given login, password and instance equal to the MOBEELIZER_MODE ("test" or "production").
        /// </summary>
        /// <param name="login">Login.</param>
        /// <param name="password">Password.</param>
        /// <param name="callback">Callback method.</param>
        public static void Login(String login, String password, MobeelizerLoginCallback callback)
        {
            Instance.Login(login, password, callback);
        }

        /// <summary>
        /// Close the user session.
        /// </summary>
        public static void Logout()
        {
            Instance.Logout();
        }

        /// <summary>
        /// Get the database for the active user session.
        /// </summary>
        /// <returns>Instance of mobeelizer database.</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static IMobeelizerDatabase GetDatabase()
        {
            return Instance.GetDatabase();
        }

        /// <summary>
        /// Start a differential sync. You can check sync status using <see cref="SyncStatusChanged"/> event.
        /// </summary>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void Sync(MobeelizerSyncCallback callback)
        {
            Instance.Sync(callback);
        }

        /// <summary>
        /// Start a full sync. All unsynced data will be lost. You can check sync status using <see cref="SyncStatusChanged"/> event.
        /// </summary>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void SyncAll(MobeelizerSyncCallback callback)
        {
            Instance.SyncAll(callback);
        }

        /// <summary>
        /// Check and return the status of current sync.
        /// </summary>
        /// <returns>Status of current sync.</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static MobeelizerSyncStatus CheckSyncStatus()
        {
            return Instance.CheckSyncStatus();
        }

        /// <summary>
        /// Create a new file with a given name and content. The returned file is ready to use as a field in the entity.
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <param name="stream">Content of the file.</param>
        /// <returns>Instance of mobeelizer file.</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static IMobeelizerFile CreateFile(String name, Stream stream)
        {
            return new MobeelizerFile(name, stream);
        }

        /// <summary>
        /// Create a file with a given name that points to a file with a given guid. Note that there is no new file created. 
        /// The returned file is ready to use as a field in the entity.
        /// </summary>
        /// <param name="name">File name.</param>
        /// <param name="guid">Existing file guid.</param>
        /// <returns>Instance of mobeelizer file.</returns>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static IMobeelizerFile CreateFile(String name, String guid)
        {
            return new MobeelizerFile(name, guid);
        }

        /// <summary>
        /// Registers device to receive push notifications.
        /// </summary>
        /// <param name="chanelUri">Notification channel uri.</param>
        /// <param name="callback">Callback method.</param>
        public static void RegisterForRemoteNotifications(String chanelUri, MobeelizerNotificationCallback callback)
        {
            Instance.RegisterForRemoteNotifications(chanelUri, callback);
        }

        /// <summary>
        /// Unregisters device from receive push notifications.
        /// </summary>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void UnregisterForRemoteNotifications(MobeelizerNotificationCallback callback)
        {
            Instance.UnregisterForRemoteNotifications(callback);
        }

        /// <summary>
        /// Sends remote notification to all users on all devices.
        /// </summary>
        /// <param name="notification">Notification to send.</param>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void SendRemoteNotification(IDictionary<String, String> notification, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(null, null, null, notification, callback);
        }

        /// <summary>
        /// Sends remote notification to all users on specified device.
        /// </summary>
        /// <param name="notification">Notification to send.</param>
        /// <param name="device">Device.</param>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void  SendRemoteNotificationToDevice(IDictionary<String, String> notification, String device, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(device, null, null, notification, callback);
        }

        /// <summary>
        /// Sends remote notification to given users.
        /// </summary>
        /// <param name="notification">Notification to send.</param>
        /// <param name="users">List of users.</param>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void  SendRemoteNotificationToUsers(IDictionary<String, String> notification, IList<String> users, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(null, null, users, notification, callback);
        }

        /// <summary>
        /// Sends remote notification to given users on specified device.
        /// </summary>
        /// <param name="notification">Notification to send.</param>
        /// <param name="users">List of users.</param>
        /// <param name="device">Device.</param>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void SendRemoteNotificationToUsersOnDevice(IDictionary<String, String> notification, IList<String> users, String device, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(device, null, users, notification, callback);
        }

        /// <summary>
        /// Sends remote notification to given group.
        /// </summary>
        /// <param name="notification">Notification to send.</param>
        /// <param name="group">Group.</param>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void SendRemoteNotificationToGroup(IDictionary<String, String> notification, String group, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(null, group, null, notification, callback);
        }

        /// <summary>
        /// Sends remote notification to given group on specified device.
        /// </summary>
        /// <param name="notification">Notification to send.</param>
        /// <param name="group">Group.</param>
        /// <param name="device">Device.</param>
        /// <param name="callback">Callback method.</param>
        /// <exception cref="System.UnauthorizedAccessException">
        /// If user session is not active.
        /// </exception>
        public static void SendRemoteNotificationToGroupOnDevice(IDictionary<String, String> notification, String group, String device, MobeelizerNotificationCallback callback)
        {
            Instance.SendRemoteNotification(device, group, null, notification, callback);
        }

        private static void OnSyncStatusChanged(MobeelizerSyncStatus status)
        {
            MobeelizerSyncStatusChangedEventHandler handler = SyncStatusChanged;
            if (handler != null)
            {
                handler(status);
            }
        }
    }
}
