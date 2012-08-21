using System;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using System.Collections.Generic;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Model;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerSyncEnumerable : IEnumerable<MobeelizerJsonEntity>
    {
        private MobeelizerDatabase mobeelizerDatabase;

        private IEnumerator<MobeelizerJsonEntity> enumerator;

        private IDictionary<String, MobeelizerModel> models;

        internal MobeelizerSyncEnumerable(MobeelizerDatabase mobeelizerDatabase, IDictionary<string, MobeelizerModel> models)
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

        public MobeelizerSyncEnumerator(MobeelizerDatabase db, IDictionary<String, MobeelizerModel> models)
        {
            this.db =  new MobeelizerDatabaseContext( db.ConnectionString);
            this.models = models;
            enumerator = (from meta in this.db.ModelMetadata where meta.ModificationLock select meta).GetEnumerator();
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
            db.Dispose();
            enumerator.Dispose();
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
