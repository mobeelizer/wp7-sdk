using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerDevelopmentConnectionManager : IMobeelizerConnectionManager
    {
        private const String SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE = "Sync is not supported in development mode.";

        private const String PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE = "Push is not supported in development mode.";

        private string developmentRole;

        internal MobeelizerDevelopmentConnectionManager(String developmentRole)
        {
            this.developmentRole = developmentRole;
        }

        public MobeelizerLoginResponse Login(bool offline)
        {
            return new MobeelizerLoginResponse(MobeelizerLoginStatus.OK, "00000000-0000-0000-0000-000000000000", developmentRole, false);
        }
        
        public string SendSyncAllRequest()
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public string SendSyncDiffRequest(Others.File outputFile)
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public Others.File GetSyncData(string ticket)
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public bool IsNetworkAvailable
        {
            get { return false; }
        }

        public void ConfirmTask(string ticket)
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public bool WaitUntilSyncRequestComplete(string ticket)
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public void RegisterForRemoteNotifications(string chanelUri)
        {
            throw new NotSupportedException(PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public void UnregisterForRemoteNotifications(string NotificationChannelUri)
        {
            throw new NotSupportedException(PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public void SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification)
        {
            throw new NotSupportedException(PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }
    }
}
