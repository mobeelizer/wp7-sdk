
namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    /// <summary>
    /// The status of current sync.
    /// </summary>
    public enum MobeelizerSyncStatus
    {
        /// <summary>
        /// Sync has not been executed in the existing user session.
        /// </summary>
        NONE,

        /// <summary>
        /// Sync is in progress. The file with local changes is being prepared.
        /// </summary>
        STARTED,

        /// <summary>
        /// Sync is in progress. The file with local changes has been prepared and now is being transmitted to the cloud.
        /// </summary>
        FILE_CREATED,

        /// <summary>
        /// Sync is in progress. The file with local changes has been transmitted to the cloud. 
        /// Waiting for the cloud to finish processing sync.
        /// </summary>
        TASK_CREATED,

        /// <summary>
        /// Sync is in progress. The file with cloud changes has been prepared and now is being transmitted to the device.
        /// </summary>
        TASK_PERFORMED,

        /// <summary>
        /// Sync is in progress. The file with cloud changes has been transmitted to the device cloud and
        /// now is being inserted into local database.
        /// </summary>
        FILE_RECEIVED,

        /// <summary>
        /// Sync has been finished successfully.
        /// </summary>
        FINISHED_WITH_SUCCESS,

        /// <summary>
        /// Sync has not been finished successfully. Look for the explanation in the application logs.
        /// </summary>
        FINISHED_WITH_FAILURE
    }

    /// <summary>
    /// Extension to the sync status.
    /// </summary>
    public static class MobeelizerSyncStatusExtension
    {
        /// <summary>
        /// Indicates whether sync task is running.
        /// </summary>
        /// <param name="status">Sync status.</param>
        /// <returns>True if sync is running.</returns>
        public static bool IsRunning(this MobeelizerSyncStatus status)
        {
            switch (status)
            {
                case MobeelizerSyncStatus.STARTED:
                case MobeelizerSyncStatus.TASK_CREATED:
                case MobeelizerSyncStatus.FILE_CREATED:
                case MobeelizerSyncStatus.FILE_RECEIVED:
                case MobeelizerSyncStatus.TASK_PERFORMED:
                    return true;
                case MobeelizerSyncStatus.NONE:
                case MobeelizerSyncStatus.FINISHED_WITH_FAILURE:
                case MobeelizerSyncStatus.FINISHED_WITH_SUCCESS:
                default:
                    return false;
            }
        }
    }
}
