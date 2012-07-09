
namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    public interface IMobeelizerConnectionService
    {
        void Authenticate(string user, string password, object remoteNotifycationToken, MobeelizerAuthenticateResponseCallback callback);

        void Authenticate(string user, string password, MobeelizerAuthenticateResponseCallback callback);

        void SendSyncAllRequest(MobeelizerSyncRequestCallback callback);

        void SendSyncDiffRequest(System.IO.IsolatedStorage.IsolatedStorageFileStream outputFile, MobeelizerSyncRequestCallback callback);

        void GetSyncData(string ticket, MobeelizerGetSyncDataCallback callback);

        void ConfirmTask(string ticket);

        bool WaitUntilSyncRequestComplete(string ticket);
    }
}
