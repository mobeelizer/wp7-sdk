using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal interface IMobeelizerConnectionService
    {
        IMobeelizerAuthenticateResponse Authenticate(string user, string password, String notificationChanelUri);

        IMobeelizerAuthenticateResponse Authenticate(string user, string password);

        MobeelizerSyncResponse SendSyncAllRequest();

        MobeelizerSyncResponse SendSyncDiffRequest(Others.File outputFile);

        MobeelizerGetSyncDataOperationResult GetSyncData(string ticket);

        void ConfirmTask(string ticket);

        MobeelizerOperationError WaitUntilSyncRequestComplete(string ticket);

        MobeelizerOperationError RegisterForRemoteNotifications(string channelUri);

        MobeelizerOperationError UnregisterForRemoteNotifications(string channelUri);

        MobeelizerOperationError SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification);
    }
}
