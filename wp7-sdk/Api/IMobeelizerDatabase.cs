using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using System.Data.Linq;

namespace Com.Mobeelizer.Mobile.Wp7.Api
{
    public interface IMobeelizerDatabase
    {
        ITable<T> GetModels<T>() where T: MobeelizerWp7Model;

        void Commit();

        // TODO:  can't be available to the user

        bool Exists(string modelName, string stringValue); 

        MobeelizerSyncEnumerable GetEntitiesToSync();

        MobeelizerSyncFileIterator GetFilesToSync();

        bool UpdateEntitiesFromSync(IEnumerable<MobeelizerJsonEntity> iList, bool isAllSynchronization); 

    }
}
