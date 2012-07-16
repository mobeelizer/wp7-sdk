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

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public class MobeelizerError
    {
        private object args;

        private string message;

        internal MobeelizerError(MobeelizerErrorCode code, string message, object args)
        {
            this.Code = code;
            this.message = message;
            this.args = args;
        }

        public MobeelizerErrorCode Code { get; private set; }

        public string Message
        {
            get
            {
                if (this.args != null)
                {
                    return String.Format(this.message, this.args);
                }

                return this.message;
            }
        }
    }
}
