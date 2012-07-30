using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Notification result.
    /// </summary>
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

        /// <summary>
        /// Returns communication status. If notification operations generates an unexpected exception, execution of  this method throws it.
        /// </summary>
        /// <returns>Comunication status.</returns>
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
