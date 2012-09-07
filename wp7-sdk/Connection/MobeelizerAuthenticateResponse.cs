using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    internal class MobeelizerAuthenticateResponse : IMobeelizerAuthenticateResponse
    {
        internal MobeelizerAuthenticateResponse(String instanceGuid, String role)
        {
            this.InstanceGuid = instanceGuid;
            this.Role = role;
        }

        internal MobeelizerAuthenticateResponse(MobeelizerOperationError error)
        {
            this.Error = error;
        }

        public string InstanceGuid { get; private set; }

        public string Role { get; private set; }

        public MobeelizerOperationError Error { get; private set; }
    }
}
