using System;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerSyncResponse
    {
        internal MobeelizerSyncResponse(String ticket)
        {
            this.Ticket = ticket;
        }

        internal MobeelizerSyncResponse(MobeelizerOperationError error)
        {
            this.Error = error;
        }

        internal String Ticket { get; private set; }

        internal MobeelizerOperationError Error { get; private set; }
    }
}
