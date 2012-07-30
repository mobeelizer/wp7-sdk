using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// Synchronizaion operation result.
    /// </summary>
    public class MobeelizerSyncResult
    {
        private MobeelizerSyncStatus status;

        private Exception exception;

        internal MobeelizerSyncResult(MobeelizerSyncStatus status)
        {
            this.status = status;
        }

        internal MobeelizerSyncResult(Exception e)
        {
            this.status = MobeelizerSyncStatus.FINISHED_WITH_FAILURE;
            this.exception = e;
        }

        /// <summary>
        /// Returns synchronization status. If sync operations generates an unexpected exception, execution of  this method throws it.
        /// </summary>
        /// <returns>Sync status.</returns>
        public MobeelizerSyncStatus GetSyncStatus()
        {
            if (this.exception != null)
            {
                throw exception;
            }

            return this.status;
        }
    }
}
