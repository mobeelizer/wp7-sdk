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

       

        public void Login(MobeelizerLoginResponseCallback callback)
        {
            bool networkConnected = IsNetworkAvailable;

            if (!networkConnected)
            {
                String[] roleAndInstanceGuid = GetRoleAndInstanceGuidFromDatabase(application);

                if (roleAndInstanceGuid[0] == null)
                {
                    Log.i(TAG, "Login failure. Missing connection failure.");          
                    callback(new MobeelizerLoginResponse(MobeelizerLoginStatus.MISSING_CONNECTION_FAILURE));
                }
                else
                {
                    Log.i(TAG, "Login '" + application.User + "' from database successful.");
                    callback(new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, roleAndInstanceGuid[1], roleAndInstanceGuid[0], false));
                }
            }

            MobeelizerAuthenticateResponseCallback authCallback = (response) =>
                {
                    if (response != null)
                    {
                        bool initialSyncRequired = IsInitialSyncRequired(application, response.InstanceGuid);

                        SetRoleAndInstanceGuidInDatabase(application, response.Role, response.InstanceGuid);
                        Log.i(TAG, "Login '" + application.User + "' successful.");
                        callback(new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, response.InstanceGuid, response.Role, initialSyncRequired));
                    }
                    else
                    {
                        Log.i(TAG, "Login failure. Authentication error.");
                        ClearRoleAndInstanceGuidInDatabase(application);
                        callback(new MobeelizerLoginResponse(MobeelizerLoginStatus.AUTHENTICATION_FAILURE));
                    }
                };

            try
            {
                if (application.RemoteNotificationToken != null)
                {
                    connectionService.Authenticate(application.User, application.Password, application.RemoteNotificationToken, authCallback);
                }
                else
                {
                    connectionService.Authenticate(application.User, application.Password, authCallback);
                }
            }
            catch (Exception) // TODO: change Exception to better one. 
            {
                String[] roleAndInstanceGuid = GetRoleAndInstanceGuidFromDatabase(application);

                if (roleAndInstanceGuid[0] == null)
                {
                    callback(new MobeelizerLoginResponse(MobeelizerLoginStatus.CONNECTION_FAILURE));
                }
                else
                {
                    callback(new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, roleAndInstanceGuid[1], roleAndInstanceGuid[0], false));
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

        public void SendSyncAllRequest(MobeelizerSyncRequestCallback callback)
        {
            try
            {
                connectionService.SendSyncAllRequest(callback);
            }
            catch (IOException e)
            {
                Log.i(TAG, e.Message);
            }
        }


        public void SendSyncDiffRequest(IsolatedStorageFileStream outputFile, MobeelizerSyncRequestCallback callback)
        {
            try
            {
                connectionService.SendSyncDiffRequest(outputFile, callback);
            }
            catch (IOException e)
            {
                Log.i(TAG, e.Message);
            }
        }

        public void GetSyncData(String ticket, MobeelizerGetSyncDataCallback callback)
        {
            try
            {
                connectionService.GetSyncData(ticket, callback);
            }
            catch (IOException e)
            {
                Log.i(TAG, e.Message);
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
