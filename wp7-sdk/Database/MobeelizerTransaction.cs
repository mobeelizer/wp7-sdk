using System;
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Data.Linq;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using System.Collections.Generic;
using System.Linq;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    internal class MobeelizerTransaction : IMobeelizerTransaction
    {
        private MobeelizerDatabaseContext dataContext;

        private MobeelizerDatabase db;

        internal MobeelizerTransaction(MobeelizerDatabase db)
        {
            dataContext = new MobeelizerDatabaseContext(db.ConnectionString);
            this.db = db;
        }

        public ITable<T> GetModelSet<T>() where T : MobeelizerWp7Model
        {
            return dataContext.GetModels<T>();
        }

        public void SubmitChanges()
        {
            ChangeSet set = dataContext.GetChangeSet();
            IList<MobeelizerModelMetadata> metadataToAdd = new List<MobeelizerModelMetadata>();

            foreach (var insert in set.Inserts)
            {
                if (insert is MobeelizerWp7Model)
                {
                    MobeelizerWp7Model modelObject = insert as MobeelizerWp7Model;
                    MobeelizerErrorsHolder errors = new MobeelizerErrorsHolder();
                    if (!db.ValidateEntity(modelObject, errors))
                    {
                        throw new ArgumentException(errors.GetErrorsSymmary());
                    }

                    String model = insert.GetType().Name;
                    String guid = Guid.NewGuid().ToString();
                    String owner = db.User;
                    modelObject.Guid = guid;
                    modelObject.Owner = owner;
                    modelObject.Conflicted = false;
                    modelObject.Deleted = false;
                    modelObject.Modified = true;
                    MobeelizerModelMetadata metadata = new MobeelizerModelMetadata()
                    {
                        Model = model,
                        Guid = guid,
                        ModificationLock = false
                    };

                    metadataToAdd.Add(metadata);
                }
            }

            foreach (var update in set.Updates)
            {
                if (update is MobeelizerWp7Model)
                {
                    MobeelizerErrorsHolder errors = new MobeelizerErrorsHolder();
                    if (!db.ValidateEntity(update as MobeelizerWp7Model, errors))
                    {
                        throw new InvalidOperationException(errors.GetErrorsSymmary());
                    }

                    String model = update.GetType().Name;
                    String guid = (update as MobeelizerWp7Model).Guid;
                    var query = from meta in dataContext.ModelMetadata where meta.Model == model && meta.Guid == guid select meta;
                    MobeelizerModelMetadata metadata = query.Single();
                    if (metadata.ModificationLock)
                    {
                        throw new InvalidOperationException("Entity is locked by synchronization process, wait until synchronization finishes.");
                    }
                    (update as MobeelizerWp7Model).Modified = true;
                }
                else if (update is MobeelizerModelMetadata)
                {
                    MobeelizerModelMetadata metadata = update as MobeelizerModelMetadata;
                    if ((update as MobeelizerWp7Model).Deleted && metadata.ModificationLock)
                    {
                        throw new InvalidOperationException("Entity is locked by synchronization process, wait until synchronization finishes.");
                    }
                }
            }

            foreach (MobeelizerModelMetadata metadata in metadataToAdd)
            {
                dataContext.ModelMetadata.InsertOnSubmit(metadata);
            }

            this.dataContext.SubmitChanges();
        }

        public void Close()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
                db.RemoveTransaction(this);
                dataContext = null;
            }
        }
    }
}
