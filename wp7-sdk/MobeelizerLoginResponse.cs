using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerLoginResponse
    {
        public MobeelizerLoginResponse(MobeelizerOperationError error)
        {
            this.Error = error;
        }

        public MobeelizerLoginResponse(MobeelizerOperationError error, string instanceGuid, string role, bool initSyncRequired)
        {
            this.Role = role;
            this.Error = error;
            this.InitialSyncRequired = initSyncRequired;
            this.InstanceGuid = instanceGuid;
        }

        public MobeelizerOperationError Error { get; private set; }

        public string Role { get; private set; }

        public string InstanceGuid { get; private set; }

        public bool InitialSyncRequired { get; private set; }
    }
}
