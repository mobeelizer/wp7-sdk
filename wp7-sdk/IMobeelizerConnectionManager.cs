using Com.Mobeelizer.Mobile.Wp7.Connection;
using System.IO.IsolatedStorage;
using System;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public interface IMobeelizerConnectionManager
    {
        void Login(MobeelizerLoginResponseCallback callback);

        void SendSyncAllRequest(MobeelizerSyncRequestCallback callback);

        void SendSyncDiffRequest(IsolatedStorageFileStream outputFile, MobeelizerSyncRequestCallback callback);

        void GetSyncData(String ticket, MobeelizerGetSyncDataCallback callback);

        bool IsNetworkAvailable { get; }

        void ConfirmTask(string ticket);

        bool WaitUntilSyncRequestComplete(string ticket);
    }
}
