
namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public enum MobeelizerSyncStatus
    {
        NONE,
        STARTED,
        FILE_CREATED,
        TASK_CREATED,
        TASK_PERFORMED,
        FILE_RECEIVED,
        FINISHED_WITH_SUCCESS,
        FINISHED_WITH_FAILURE
    }

    public static class MobeelizerSyncStatusExtension
    {
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
