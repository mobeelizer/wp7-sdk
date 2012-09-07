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
            return new MobeelizerLoginResponse(null, "00000000-0000-0000-0000-000000000000", developmentRole, false);
        }

        public MobeelizerSyncResponse SendSyncAllRequest()
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public MobeelizerSyncResponse SendSyncDiffRequest(Others.File outputFile)
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public MobeelizerGetSyncDataOperationResult GetSyncData(string ticket)
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

        public MobeelizerOperationError WaitUntilSyncRequestComplete(string ticket)
        {
            throw new NotSupportedException(SYNC_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public MobeelizerOperationError RegisterForRemoteNotifications(string chanelUri)
        {
            throw new NotSupportedException(PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public MobeelizerOperationError UnregisterForRemoteNotifications(string NotificationChannelUri)
        {
            throw new NotSupportedException(PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }

        public MobeelizerOperationError SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification)
        {
            throw new NotSupportedException(PUSH_IS_NOT_SUPPORTED_IN_DEVELOPMENT_MODE);
        }
    }
}
