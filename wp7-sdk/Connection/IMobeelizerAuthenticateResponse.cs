using System;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal interface IMobeelizerAuthenticateResponse
    {

        String InstanceGuid { get; }

        String Role { get; }
    }
}
