using Com.Mobeelizer.Mobile.Wp7.Connection;
using System.IO.IsolatedStorage;
using System;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal interface IMobeelizerConnectionManager
    {
        MobeelizerLoginResponse Login();

        String SendSyncAllRequest();

        String SendSyncDiffRequest(Others.File outputFile);

        Others.File GetSyncData(String ticket);

        bool IsNetworkAvailable { get; }

        void ConfirmTask(string ticket);

        bool WaitUntilSyncRequestComplete(string ticket);
    }
}
