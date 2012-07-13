using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Diagnostics;
using Com.Mobeelizer.Mobile.Wp7.Connection;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerRealConnectionManager : IMobeelizerConnectionManager
    {
        private const String TAG = "mobeelizer:mobeelizerrealconnectionmanager";

        private MobeelizerApplication application;

        private IMobeelizerConnectionService connectionService;

        public MobeelizerRealConnectionManager(MobeelizerApplication application)
        {
            this.application = application;
            this.connectionService = new MobeelizerConnectionService(application);
        }

        public MobeelizerLoginResponse Login()
        {
            bool networkConnected = IsNetworkAvailable;

            if (!networkConnected)
            {
                String[] roleAndInstanceGuid = GetRoleAndInstanceGuidFromDatabase(application);

                if (roleAndInstanceGuid[0] == null)
                {
                    Log.i(TAG, "Login failure. Missing connection failure.");          
                    return new MobeelizerLoginResponse(MobeelizerLoginStatus.MISSING_CONNECTION_FAILURE);
                }
                else
                {
                    Log.i(TAG, "Login '" + application.User + "' from database successful.");
                    return new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, roleAndInstanceGuid[1], roleAndInstanceGuid[0], false);
                }
            }

            try
            {
                IMobeelizerAuthenticateResponse response = null;
                if (application.RemoteNotificationToken != null)
                {
                     response = connectionService.Authenticate(application.User, application.Password, application.RemoteNotificationToken);
                }
                else
                {
                    response = connectionService.Authenticate(application.User, application.Password);
                }

                if (response != null)
                {
                    bool initialSyncRequired = IsInitialSyncRequired(application, response.InstanceGuid);

                    SetRoleAndInstanceGuidInDatabase(application, response.Role, response.InstanceGuid);
                    Log.i(TAG, "Login '" + application.User + "' successful.");
                    return new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, response.InstanceGuid, response.Role, initialSyncRequired);
                }
                else
                {
                    Log.i(TAG, "Login failure. Authentication error.");
                    ClearRoleAndInstanceGuidInDatabase(application);
                    return new MobeelizerLoginResponse(MobeelizerLoginStatus.AUTHENTICATION_FAILURE);
                }
            }
            catch (InvalidOperationException e)
            {
                Log.i(TAG, e.Message);
                String[] roleAndInstanceGuid = GetRoleAndInstanceGuidFromDatabase(application);
                if (roleAndInstanceGuid[0] == null)
                {
                    return new MobeelizerLoginResponse(MobeelizerLoginStatus.CONNECTION_FAILURE);
                }
                else
                {
                    return new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, roleAndInstanceGuid[1], roleAndInstanceGuid[0], false);
                }
            }
        }

        private bool IsInitialSyncRequired(MobeelizerApplication application, string instanceGuid)
        {
            return application.InternalDatabase.IsInitialSyncRequired(application.Instance, instanceGuid, application.User);
        }

        private void ClearRoleAndInstanceGuidInDatabase(MobeelizerApplication application)
        {
            application.InternalDatabase.ClearRoleAndInstanceGuid(application.Instance, application.User);
        }

        private void SetRoleAndInstanceGuidInDatabase(MobeelizerApplication application, string role, string instanceGuid)
        {
            application.InternalDatabase.SetRoleAndInstanceGuid(application.Instance, application.User, application.Password, role, instanceGuid);
        }

        private string[] GetRoleAndInstanceGuidFromDatabase(MobeelizerApplication application)
        {
            return application.InternalDatabase.GetRoleAndInstanceGuid(application.Instance, application.User, application.Password);
        }

        public bool IsNetworkAvailable
        {
            get
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        public String SendSyncAllRequest()
        {
            try
            {
                return connectionService.SendSyncAllRequest();
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }


        public String SendSyncDiffRequest(Others.File outputFile)
        {
            try
            {
                return connectionService.SendSyncDiffRequest(outputFile);
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public Others.File GetSyncData(String ticket)
        {
            try
            {
                return connectionService.GetSyncData(ticket);
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public void ConfirmTask(String ticket)
        {
            try
            {
                connectionService.ConfirmTask(ticket);
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }

        public bool WaitUntilSyncRequestComplete(String ticket)
        {
            try
            {
                return connectionService.WaitUntilSyncRequestComplete(ticket);
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }
        }
    }
}
