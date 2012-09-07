using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal interface IMobeelizerConnectionManager
    {
        MobeelizerLoginResponse Login(bool offline);

        MobeelizerSyncResponse SendSyncAllRequest();

        MobeelizerSyncResponse SendSyncDiffRequest(Others.File outputFile);

        MobeelizerGetSyncDataOperationResult GetSyncData(String ticket);

        bool IsNetworkAvailable { get; }

        void ConfirmTask(string ticket);

        MobeelizerOperationError WaitUntilSyncRequestComplete(string ticket);

        MobeelizerOperationError RegisterForRemoteNotifications(string chanelUri);

        MobeelizerOperationError UnregisterForRemoteNotifications(string NotificationChannelUri);

        MobeelizerOperationError SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification);
    }
}
