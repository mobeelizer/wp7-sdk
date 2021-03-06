﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Configuration;
using Com.Mobeelizer.Mobile.Wp7.Database;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using Com.Mobeelizer.Mobile.Wp7.Model;
using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerApplication
    {
        private const String TAG = "mobeelizer";

        private const String META_DEVICE = "device";

        private const String META_URL = "mobeelizerUrl";

        private const String META_PACKAGE = "entitiesNamespace";

        private const String META_DEFINITION_ASSET = "definitionAsset";

        private const String META_DATABASE_VERSION = "databaseVersion";

        private const String META_MODE = "mode";

        private const String META_DEVELOPMENT_ROLE = "developmentRole";

        private String device;

        private String entityPackage;

        private int databaseVersion;

        private String url;

        private MobeelizerMode mode;

        private String deviceIdentifier;

        private MobeelizerApplicationDefinition definition;

        private String vendor;

        private String application;

        private String versionDigest;

        private MobeelizerInternalDatabase internalDatabase;

        private IMobeelizerConnectionManager connectionManager;

        private MobeelizerDefinitionConverter definitionConverter = new MobeelizerDefinitionConverter();

        private string instance;

        private string user;

        private string password;

        private string role;

        private string instanceGuid;

        private bool loggedIn;

        private MobeelizerSyncStatus syncStatus;

        private MobeelizerFileService fileService;

        private MobeelizerDatabase database;

        private MobeelizerTombstoningManager tombstoningManager;

        internal event MobeelizerSyncStatusChangedEventHandler SyncStatusChanged;

        internal static MobeelizerApplication CreateApplication()
        {
            MobeelizerApplication application = new MobeelizerApplication();
            application.tombstoningManager = new MobeelizerTombstoningManager(application);
            MobeelizerConfigurationSection section = (MobeelizerConfigurationSection)ConfigurationManager.GetSection("mobeelizer-configuration");
            if (section == null)
            {
                throw new ConfigurationException("'mobeelizer-configuration' section not found in app.config file. Check if app.config is not missing and if file Build Action is set to Content.");
            }

            String device;
            String entityPackage;
            String definitionXml;
            String developmentRole;
            String url;
            String stringMode;
            int databaseVersion = 1;
            try
            {
                device = section.AppSettings[META_DEVICE].Value;
            }
            catch (KeyNotFoundException)
            {
                throw new ConfigurationException(META_DEVICE + " must be set in app.config file.");
            }

            try
            {
                entityPackage = section.AppSettings[META_PACKAGE].Value;
            }
            catch (KeyNotFoundException)
            {
                throw new ConfigurationException(META_PACKAGE + " must be set in app.config file.");
            }

            try
            {
                definitionXml = section.AppSettings[META_DEFINITION_ASSET].Value;
            }
            catch (KeyNotFoundException) { definitionXml = "application.xml"; }

            try
            {
                developmentRole = section.AppSettings[META_DEVELOPMENT_ROLE].Value;
            }
            catch (KeyNotFoundException) { developmentRole = null; }

            try
            {
                String strDatabaseVersion = section.AppSettings[META_DATABASE_VERSION].Value;
                if (!Int32.TryParse(strDatabaseVersion, out databaseVersion))
                {
                    throw new ConfigurationException(META_DATABASE_VERSION + " must be natural number.");
                }
            }
            catch (KeyNotFoundException) { databaseVersion = 1; }

            try
            {
                url = section.AppSettings[META_URL].Value;
            }
            catch (KeyNotFoundException) { url = null; }

            try
            {
                stringMode = section.AppSettings[META_MODE].Value;
            }
            catch (KeyNotFoundException)
            {
                throw new ConfigurationException(META_MODE + " must be set in app.config file.");
            }

            application.initApplication(device, entityPackage, developmentRole, definitionXml, databaseVersion, url, stringMode);
            return application;
        }

        private void initApplication(string device, string entityPackage, string developmentRole, string definitionXml, int databaseVersion, string url, string stringMode)
        {
            Log.i(TAG, "Creating Mobeelizer SDK ", Mobeelizer.VERSION);

            this.device = device;
            this.entityPackage = entityPackage;
            this.databaseVersion = databaseVersion;
            this.url = url;

            if (stringMode == null)
            {
                this.mode = MobeelizerMode.DEVELOPMENT;
            }
            else
            {
                this.mode = (MobeelizerMode)Enum.Parse(typeof(MobeelizerMode), stringMode, true);
            }

            if (this.mode == MobeelizerMode.DEVELOPMENT && developmentRole == null)
            {
                throw new ConfigurationException(META_DEVELOPMENT_ROLE + " must be set in development MobeelizerMode.");
            }

            try
            {
                byte[] id = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
                deviceIdentifier = Convert.ToBase64String(id);
            }
            catch (UnauthorizedAccessException)
            {
                throw new ConfigurationException("Could to resolve device identifier, check app capabilities - ID_CAP_IDENTITY_DEVICE is required.");
            }

            if (mode == MobeelizerMode.DEVELOPMENT)
            {
                connectionManager = new MobeelizerDevelopmentConnectionManager(developmentRole);
            }
            else
            {
                connectionManager = new MobeelizerRealConnectionManager(this);
            }

            try
            {
                definition = MobeelizerDefinitionParser.Parse(XDocument.Load(definitionXml));
            }
            catch (XmlException e)
            {
                throw new ConfigurationException("Cannot read definition from " + definitionXml + ".", e);
            }

            vendor = definition.Vendor;
            application = definition.Application;
            versionDigest = definition.Digest;
            internalDatabase = new MobeelizerInternalDatabase();
            fileService = new MobeelizerFileService(this);
        }

        internal MobeelizerFileService GetFileService()
        {
            return this.fileService;
        }

        internal void Logout()
        {
            if (!IsLoggedIn)
            {
                return;
            }

            if (CheckSyncStatus().IsRunning())
            {
                throw new SystemException("Cannot logout when sync is in progress.");
            }

            Log.i(TAG, "logout");

            this.instance = null;
            this.user = null;
            this.password = null;

            if (database != null)
            {
                database.Close();
                database = null;
            }

            loggedIn = false;
        }

        internal void Login(string instance, string user, string password, MobeelizerOperationCallback callback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                MobeelizerOperationError error = null;
                try
                {
                    error = Login(instance, user, password, false);
                }
                catch (Exception e)
                {
                    Log.i(TAG, e.Message);
                    error = MobeelizerOperationError.Exception(e);
                }

                callback(error);
            }));

            thread.Name = "Mobeelizer login thread";
            thread.Start();
        }

        internal void Login(string user, string password, MobeelizerOperationCallback callback)
        {
            Login(mode == MobeelizerMode.PRODUCTION ? "production" : "test", user, password, callback);
        }

        internal MobeelizerOperationError OfflineLogin(String instance, String user, String password)
        {
            return Login(instance, user, password, true);
        }

        private MobeelizerOperationError Login(string instance, string user, string password, bool offline)
        {
            if (IsLoggedIn)
            {
                Logout();
            }

            Log.i(TAG, "login: " + vendor + ", " + application + ", " + instance + ", " + user + ", " + password);
            this.instance = instance;
            this.user = user;
            this.password = password;
            MobeelizerLoginResponse response = connectionManager.Login(offline);
            Log.i(TAG, "Login result: " + response.Error + ", " + response.Role + ", " + response.InstanceGuid);
            if (response.Error != null)
            {
                this.instance = null;
                this.user = null;
                this.password = null;
                return response.Error;
            }
            else
            {
                role = response.Role;
                instanceGuid = response.InstanceGuid;
                loggedIn = true;
                IDictionary<String, MobeelizerModel> models = new Dictionary<String, MobeelizerModel>();
                foreach (MobeelizerModel model in definitionConverter.Convert(definition, entityPackage, role))
                {
                    models.Add(model.Name, model);
                }

                database = new MobeelizerDatabase(this, models);
                database.Open();
                if (response.InitialSyncRequired)
                {
                    Sync(true);
                }

                return null;
            }
        }

        internal bool IsLoggedIn
        {
            get
            {
                return this.loggedIn;
            }
        }

        internal MobeelizerDatabase GetDatabase()
        {
            CheckIfLoggedIn();
            return this.database;
        }

        internal void Sync(MobeelizerOperationCallback callback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                MobeelizerOperationError error = null;
                try
                {
                    error = Sync();
                }
                catch (Exception e)
                {
                    Log.i(TAG, e.Message);
                    this.SetSyncStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                    error = MobeelizerOperationError.Exception(e);
                }
                callback(error);
            }));
            thread.Name = "Mobeelizer synchronization thread";
            thread.Start();
        }

        private MobeelizerOperationError Sync()
        {
            CheckIfLoggedIn();
            Log.i(TAG, "Truncate data and start sync service.");
            return Sync(false);
        }

        internal void SyncAll(MobeelizerOperationCallback callback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    callback(SyncAll());
                }
                catch (Exception e)
                {
                    Log.i(TAG, e.Message);
                    this.SetSyncStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                    callback(MobeelizerOperationError.Exception(e));
                }
            }));
            thread.Name = "Mobeelizer full synchronization thread";
            thread.Start();
        }

        private MobeelizerOperationError SyncAll()
        {
            CheckIfLoggedIn();
            Log.i(TAG, "Truncate data and start sync service.");
            return Sync(true);
        }

        private MobeelizerOperationError Sync(bool syncAll)
        {
            if (mode == MobeelizerMode.DEVELOPMENT || CheckSyncStatus().IsRunning())
            {
                Log.i(TAG, "Sync is already running - skipping.");
                return null;
            }
            else if (!connectionManager.IsNetworkAvailable)
            {
                Log.i(TAG, "Sync cannot be performed - network is not available.");
                SetSyncStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                return MobeelizerOperationError.MissingConnectionError();
            }
            else
            {
                SetSyncStatus(MobeelizerSyncStatus.STARTED);
                return new MobeelizerSyncServicePerformer(Mobeelizer.Instance, syncAll).Sync();
            }
        }

        internal void SetSyncStatus(MobeelizerSyncStatus mobeelizerSyncStatus)
        {
            MobeelizerSyncStatusChangedEventHandler handler = SyncStatusChanged;
            if (handler != null)
            {
                handler(mobeelizerSyncStatus);
            }

            this.syncStatus = mobeelizerSyncStatus;
        }

        private void CheckIfLoggedIn()
        {
            if (!IsLoggedIn)
            {
                throw new UnauthorizedAccessException("User is not logged in.");
            }
        }

        internal string NotificationChannelUri { get; set; }

        internal MobeelizerSyncStatus CheckSyncStatus()
        {
            CheckIfLoggedIn();
            Log.i(TAG, "Check sync status.");
            if (mode == MobeelizerMode.DEVELOPMENT)
            {
                return MobeelizerSyncStatus.NONE;
            }

            return syncStatus;
        }

        internal string User
        {
            get
            {
                return this.user;
            }
        }

        internal MobeelizerInternalDatabase InternalDatabase
        {
            get
            {
                return this.internalDatabase;
            }
        }

        internal string Instance
        {
            get
            {
                return this.instance;
            }
        }

        internal string Password
        {
            get
            {
                return this.password;
            }
        }

        internal string Url
        {
            get
            {
                return this.url;
            }
        }

        internal MobeelizerMode Mode
        {
            get
            {
                return this.mode;
            }
        }

        internal string Vendor
        {
            get
            {
                return this.vendor;
            }
        }

        internal string Application
        {
            get
            {
                return this.application;
            }
        }

        internal string Digest
        {
            get
            {
                return this.versionDigest;
            }
        }

        internal string Device
        {
            get
            {
                return this.device;
            }
        }

        internal string DeviceIdentifier
        {
            get
            {
                return this.deviceIdentifier;
            }
        }

        public int DataBaseVersion
        {
            get
            {
                return this.databaseVersion;
            }
        }

        internal IMobeelizerConnectionManager GetConnectionManager()
        {
            return connectionManager;
        }

        internal string InstanceGuid
        {
            get
            {
                return this.instanceGuid;
            }
        }

        internal MobeelizerTombstoningManager GetTombstoningManager()
        {
            return this.tombstoningManager;
        }

        internal void RegisterForRemoteNotifications(string chanelUri, MobeelizerOperationCallback callback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
                {
                    MobeelizerOperationError error = null;
                    try
                    {
                        NotificationChannelUri = chanelUri;
                        if (IsLoggedIn)
                        {
                           error = connectionManager.RegisterForRemoteNotifications(chanelUri);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.i(TAG, e.Message);
                        error = MobeelizerOperationError.Exception(e);
                    }

                    callback(error);
                }));
            thread.Name = "Register for notification thread";
            thread.Start();
        }

        internal void UnregisterForRemoteNotifications(MobeelizerOperationCallback callback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
                {
                    MobeelizerOperationError error = null;
                    try
                    {
                        CheckIfLoggedIn();
                        error = connectionManager.UnregisterForRemoteNotifications(NotificationChannelUri);
                    }
                    catch (Exception e)
                    {
                        Log.i(TAG, e.Message);
                        error = MobeelizerOperationError.Exception(e);
                    }

                    callback(error);
                }));
            thread.Name = "Register for notification thread";
            thread.Start();
        }

        internal void SendRemoteNotification(String device, String group, IList<String> users, IDictionary<String, String> notification, MobeelizerOperationCallback callback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
                {
                    MobeelizerOperationError error;
                    try
                    {
                        CheckIfLoggedIn();
                        error = connectionManager.SendRemoteNotification(device, group, users, notification);
                    }
                    catch (Exception e)
                    {
                        Log.i(TAG, e.Message);
                        error = MobeelizerOperationError.Exception(e);
                    }

                    callback(error);
                }));
            thread.Name = "Send notification thread";
            thread.Start();
        }
    }
}
