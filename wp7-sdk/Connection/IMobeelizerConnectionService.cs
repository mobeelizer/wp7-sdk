using System;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal interface IMobeelizerConnectionService
    {
        IMobeelizerAuthenticateResponse Authenticate(string user, string password, String notificationChanelUri);

        IMobeelizerAuthenticateResponse Authenticate(string user, string password);

        String SendSyncAllRequest();

        String SendSyncDiffRequest(Others.File outputFile);

        Others.File GetSyncData(string ticket);

        void ConfirmTask(string ticket);

        bool WaitUntilSyncRequestComplete(string ticket);

        void RegisterForRemoteNotifications(string channelUri);

        void UnregisterForRemoteNotifications(string channelUri);

        void SendRemoteNotification(string device, string group, System.Collections.Generic.IList<string> users, System.Collections.Generic.IDictionary<string, string> notification);
    }
}
