
using System;
namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    public interface IMobeelizerConnectionService
    {
        IMobeelizerAuthenticateResponse Authenticate(string user, string password, object remoteNotifycationToken);

        IMobeelizerAuthenticateResponse Authenticate(string user, string password);

        String SendSyncAllRequest();

        String SendSyncDiffRequest(Others.File outputFile);

        Others.File GetSyncData(string ticket);

        void ConfirmTask(string ticket);

        bool WaitUntilSyncRequestComplete(string ticket);
    }
}
