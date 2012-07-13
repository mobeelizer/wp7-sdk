using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Configuration;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using Microsoft.Practices.Mobile.Configuration;
using System.Threading;
using Com.Mobeelizer.Mobile.Wp7.Model;
using Com.Mobeelizer.Mobile.Wp7.Database;


namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerApplication
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

        internal static MobeelizerApplication CreateApplication()
        {
            MobeelizerApplication application = new MobeelizerApplication();
            MobeelizerConfigurationSection section = (MobeelizerConfigurationSection)ConfigurationManager.GetSection("mobeelizer-configuration");
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
            catch(KeyNotFoundException) { databaseVersion = 1; }

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

            //this.mobeelizer = mobeelizer;
            //Mobeelizer.setInstance(this);

            //String state = Environment.getExternalStorageState();

            //if (!Environment.MEDIA_MOUNTED.equals(state))
            //{
            //    if (Environment.MEDIA_MOUNTED_READ_ONLY.equals(state))
            //    {
            //        throw new IllegalStateException("External storage must be available and read-only.");
            //    }
            //    else
            //    {
            //        throw new IllegalStateException("External storage must be available.");
            //    }
            //}

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
                //TODO:
               // connectionManager = new MobeelizerDevelopmentConnectionManager(developmentRole);
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

            fileService = new MobeelizerFileService();
        }

        internal MobeelizerFileService GetFileService()
        {
            return this.fileService;
        }

        internal void Logout()
        {
            if (!IsLoggedIn)
            {
                return; // ignore
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

        internal MobeelizerLoginStatus Login(string instance, string user, string password)
        {
            if (IsLoggedIn)
            {
                Logout();
            }

            Log.i(TAG, "login: " + vendor + ", " + application + ", " + instance + ", " + user + ", " + password);

            this.instance = instance;
            this.user = user;
            this.password = password;

            MobeelizerLoginResponse status =  connectionManager.Login();
                Log.i(TAG, "Login result: " + status.Status + ", " + status.Role + ", " + status.InstanceGuid);

                if (status.Status != MobeelizerLoginStatus.OK)
                {
                    this.instance = null;
                    this.user = null;
                    this.password = null;
                    return status.Status;
                }
                else
                {

                    role = status.Role;
                    instanceGuid = status.InstanceGuid;

                    loggedIn = true;

                    IDictionary<String, MobeelizerModel> models = new Dictionary<String, MobeelizerModel>();

                    foreach (MobeelizerModel model in definitionConverter.Convert(definition, entityPackage, role))
                    {
                        models.Add(model.Name, model);
                    }

                    database = new MobeelizerDatabase(this, models);
                    database.Open();

                    if (status.InitialSyncRequired)
                    {
                        Sync(true);
                    }

                    return MobeelizerLoginStatus.OK;
                }
        }

        internal MobeelizerLoginStatus Login(string user, string password)
        {
            return Login(mode == MobeelizerMode.PRODUCTION ? "production" : "test", user, password);
        }

        internal bool IsLoggedIn
        {
            get
            {
                return this.loggedIn;
            }
        }

        internal IMobeelizerDatabase GetDatabase()
        {
            CheckIfLoggedIn();
            
            return this.database;
        }

        internal MobeelizerSyncStatus Sync()
        {
            CheckIfLoggedIn();
            Log.i(TAG, "Truncate data and start sync service.");
            return Sync(false);
        }

        internal MobeelizerSyncStatus SyncAll()
        {
            CheckIfLoggedIn();
            Log.i(TAG, "Truncate data and start sync service.");
            return Sync(true);
        }

        private MobeelizerSyncStatus Sync(bool syncAll)
        {
            if (mode == MobeelizerMode.DEVELOPMENT || CheckSyncStatus().IsRunning())
            {
                Log.i(TAG, "Sync is already running - skipping.");
                return MobeelizerSyncStatus.NONE;
            }
            else if (!connectionManager.IsNetworkAvailable)
            {
                Log.i(TAG, "Sync cannot be performed - network is not available.");
                SetSyncStatus(MobeelizerSyncStatus.FINISHED_WITH_FAILURE);
                return MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
            }
            else
            {
                SetSyncStatus(MobeelizerSyncStatus.STARTED);
                return new MobeelizerSyncServicePerformer(Mobeelizer.Instance, syncAll).Sync();
            }
        }

        internal void SetSyncStatus(MobeelizerSyncStatus mobeelizerSyncStatus)
        {
            this.syncStatus = mobeelizerSyncStatus;
        }

        private void CheckIfLoggedIn()
        {
            if (!IsLoggedIn)
            {
                throw new InvalidOperationException("User is not logged in.");
            }
        }

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

        public string User
        {
            get
            {
                return this.user;
            }
        }

        public MobeelizerInternalDatabase InternalDatabase
        {
            get
            {
                return this.internalDatabase;
            }
        }

        public string Instance
        {
            get
            {
                return this.instance;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
        }

        public object RemoteNotificationToken { get; set; } // TODO

        public string Url
        {
            get
            {
                return this.url;
            }
        }

        public MobeelizerMode Mode {

            get
            {
                return this.mode;
            }
        }

        public string Vendor
        {
            get
            {
                return this.vendor;
            }
        }

        public string Application
        {
            get
            {
                return this.application;
            }
        }

        public string Digest
        {
            get
            {
                return this.versionDigest;
            }
        }

        public string Device
        {
            get
            {
                return this.device;
            }
        }

        public string DeviceIdentifier
        {
            get
            {
                return this.deviceIdentifier;
            }
        }

        internal IMobeelizerConnectionManager GetConnectionManager()
        {
            return connectionManager;
        }

        public string InstanceGuid
        {
            get
            {
                return this.instanceGuid;
            }
        }
    }
}
