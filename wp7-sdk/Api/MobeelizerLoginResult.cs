using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public class MobeelizerLoginResult
    {
        private MobeelizerLoginStatus status;

        private Exception exception;

        internal MobeelizerLoginResult(MobeelizerLoginStatus status)
        {
            this.status = status;
        }

        internal MobeelizerLoginResult(Exception e)
        {
            this.status = MobeelizerLoginStatus.OTHER_FAILURE;
            this.exception = e;
        }

        public MobeelizerLoginStatus GetLoginStatus()
        {
            if (this.exception != null)
            {
                throw exception;
            }

            return this.status;
        }
    }
}
