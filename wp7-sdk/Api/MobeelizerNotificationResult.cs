using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public class MobeelizerNotificationResult
    {
        private MobeelizerCommunicationStatus status;

        private Exception exception;

        internal MobeelizerNotificationResult(MobeelizerCommunicationStatus status)
        {
            this.status = status;
        }

        internal MobeelizerNotificationResult(Exception e)
        {
            this.status = MobeelizerCommunicationStatus.FAILURE;
            this.exception = e;
        }

        public MobeelizerCommunicationStatus GetCommunicationStatus()
        {
            if (this.exception != null)
            {
                throw exception;
            }

            return this.status;
        }
    }
}
