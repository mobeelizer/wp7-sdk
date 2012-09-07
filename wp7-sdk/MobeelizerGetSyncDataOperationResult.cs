using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerGetSyncDataOperationResult
    {
        internal MobeelizerGetSyncDataOperationResult(Others.File inputFile)
        {
            this.InputFile = inputFile;
        }

        internal MobeelizerGetSyncDataOperationResult(MobeelizerOperationError error)
        {
            this.Error = error;
        }

        internal Others.File InputFile { get; private set; }

        internal MobeelizerOperationError Error { get; private set; }
    }
}
