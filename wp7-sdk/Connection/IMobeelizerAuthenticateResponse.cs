using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal interface IMobeelizerAuthenticateResponse
    {
        String InstanceGuid { get; }

        String Role { get; }

        MobeelizerOperationError Error { get; }
    }
}
