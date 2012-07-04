using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Configuration;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7
{
    class MobeelizerApplication
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
            Debug.WriteLine("{0}\t{1}\tv{2}", TAG, "Creating Mobeelizer SDK ", Mobeelizer.VERSION);

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
               // connectionManager = new MobeelizerDevelopmentConnectionManager(developmentRole);
            }
            else
            {
                //connectionManager = new MobeelizerRealConnectionManager(this);
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
            versionDigest = definition.DigestString;

            //internalDatabase = new MobeelizerInternalDatabase(this);

            //fileService = new MobeelizerFileService(this);
        }

        internal void Logout()
        {
            throw new NotImplementedException();
        }

        internal MobeelizerLoginStatus Login(string instance, string login, string password)
        {
            throw new NotImplementedException();
        }

        internal void Login(string instance, string login, string password, MobeelizerLoginCallback callback)
        {
            throw new NotImplementedException();
        }

        internal MobeelizerLoginStatus Login(string login, string password)
        {
            throw new NotImplementedException();
        }

        internal void Login(string login, string password, MobeelizerLoginCallback callback)
        {
            throw new NotImplementedException();
        }

        internal bool IsLoggedIn { get; set; }

        internal MobeelizerDatabase GetDatabase()
        {
            throw new NotImplementedException();
        }

        internal void Sync(MobeelizerSyncCallback callback)
        {
            throw new NotImplementedException();
        }

        internal MobeelizerSyncStatus Sync()
        {
            throw new NotImplementedException();
        }

        internal void SyncAll(MobeelizerSyncCallback callback)
        {
            throw new NotImplementedException();
        }

        internal MobeelizerSyncStatus SyncAll()
        {
            throw new NotImplementedException();
        }

        internal MobeelizerSyncStatus CheckSyncStatus()
        {
            throw new NotImplementedException();
        }
    }
}
