using System;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using System.Collections.Generic;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Model;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7
{
    public class MobeelizerSyncEnumerable : IEnumerable<MobeelizerJsonEntity>
    {
        private MobeelizerDatabaseContext mobeelizerDatabase;

        private IEnumerator<MobeelizerJsonEntity> enumerator;

        private IDictionary<String, MobeelizerModel> models;

        public MobeelizerSyncEnumerable(MobeelizerDatabaseContext mobeelizerDatabase, IDictionary<String, MobeelizerModel> models)
        {
            this.mobeelizerDatabase = mobeelizerDatabase;
            this.models = models;
            enumerator = new MobeelizerSyncEnumerator(mobeelizerDatabase, models);            
        }

        public IEnumerator<MobeelizerJsonEntity> GetEnumerator()
        {
            return this.enumerator;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    internal class MobeelizerSyncEnumerator : IEnumerator<MobeelizerJsonEntity>
    {
        private MobeelizerDatabaseContext db;

        private IEnumerator<MobeelizerModelMetadata> enumerator;

        private IDictionary<String, MobeelizerModel> models;

        public MobeelizerSyncEnumerator(MobeelizerDatabaseContext db, IDictionary<String, MobeelizerModel> models)
        {
            this.db = db;
            this.models = models;
            enumerator = (from meta in this.db.ModelMetadata where meta.Modyfied == 2 select meta).GetEnumerator();
        }

        public MobeelizerJsonEntity Current
        {
            get 
            {
                MobeelizerModelMetadata metadata = enumerator.Current;
                return this.models[metadata.Model].GetJsonEntity(metadata, db);
            }
        }

        public void Dispose()
        {
            enumerator.Dispose();
            db = null;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            this.enumerator.Reset();
        }
    }
}
