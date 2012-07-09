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

namespace Com.Mobeelizer.Mobile.Wp7.Connection
{
    public class MobeelizerAuthenticateResponse : IMobeelizerAuthenticateResponse
    {
        public MobeelizerAuthenticateResponse(String instanceGuid, String role)
        {
            this.InstanceGuid = instanceGuid;
            this.Role = role;
        }

        public string InstanceGuid { get; private set; }

        public string Role { get; private set; }
    }
}
