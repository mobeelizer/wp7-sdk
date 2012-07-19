using System;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
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
