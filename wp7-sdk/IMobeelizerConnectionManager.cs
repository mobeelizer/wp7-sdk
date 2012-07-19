using System;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal interface IMobeelizerConnectionManager
    {
        MobeelizerLoginResponse Login(bool offline);

        String SendSyncAllRequest();

        String SendSyncDiffRequest(Others.File outputFile);

        Others.File GetSyncData(String ticket);

        bool IsNetworkAvailable { get; }

        void ConfirmTask(string ticket);

        bool WaitUntilSyncRequestComplete(string ticket);

        void RegisterForRemoteNotifications(string chanelUri);

        void UnregisterForRemoteNotifications(string NotificationChannelUri);

        void SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification);
    }
}
