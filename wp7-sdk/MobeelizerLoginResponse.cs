using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerLoginResponse
    {
        public MobeelizerLoginResponse(MobeelizerLoginStatus status)
        {
            this.Status = status;
        }

        public MobeelizerLoginResponse(MobeelizerLoginStatus status, string instanceGuid, string role, bool initSyncRequired)
        {
            this.Role = role;
            this.Status = status;
            this.InitialSyncRequired = initSyncRequired;
            this.InstanceGuid = instanceGuid;
        }

        public MobeelizerLoginStatus Status { get; private set; }

        public string Role { get; private set; }

        public string InstanceGuid { get; private set; }

        public bool InitialSyncRequired { get; private set; }
    }
}
