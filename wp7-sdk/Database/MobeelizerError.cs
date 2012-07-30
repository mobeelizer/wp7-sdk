using System;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    internal class MobeelizerError
    {
        private object args;

        private string message;

        internal MobeelizerError(MobeelizerErrorCode code, string message, object args)
        {
            this.Code = code;
            this.message = message;
            this.args = args;
        }

        internal MobeelizerErrorCode Code { get; private set; }

        internal string Message
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
