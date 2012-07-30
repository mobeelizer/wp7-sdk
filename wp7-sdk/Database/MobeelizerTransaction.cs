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

        public ITable<T> GetModels<T>() where T : MobeelizerWp7Model
        {
            return dataContext.GetModels<T>();
        }

        public void Commit()
        {
            ChangeSet set = dataContext.GetChangeSet();
            IList<MobeelizerModelMetadata> metadataToAdd = new List<MobeelizerModelMetadata>();
            IList<MobeelizerModelMetadata> metadataToUpdate = new List<MobeelizerModelMetadata>();
            IList<MobeelizerModelMetadata> recordsToDelete = new List<MobeelizerModelMetadata>();

            foreach (var insert in set.Inserts)
            {
                if (insert is MobeelizerWp7Model)
                {
                    MobeelizerErrorsHolder errors = new MobeelizerErrorsHolder();
                    if (!db.ValidateEntity(insert as MobeelizerWp7Model, errors))
                    {
                        throw new ArgumentException(errors.GetErrorsSymmary());
                    }

                    String model = insert.GetType().Name;
                    String guid = Guid.NewGuid().ToString();
                    (insert as MobeelizerWp7Model).guid = guid;
                    String owner = db.User;
                    MobeelizerModelMetadata metadata = new MobeelizerModelMetadata()
                    {
                        Model = model,
                        Guid = guid,
                        Owner = owner,
                        Conflicted = 0,
                        Deleted = 0,
                        Modyfied = 1
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
                    String guid = (update as MobeelizerWp7Model).guid;
                    var query = from meta in dataContext.ModelMetadata where meta.Model == model && meta.Guid == guid select meta;
                    MobeelizerModelMetadata metadata = query.Single();
                    metadataToUpdate.Add(metadata);
                }
            }

            foreach (MobeelizerModelMetadata metadata in metadataToAdd)
            {
                dataContext.ModelMetadata.InsertOnSubmit(metadata);
            }

            foreach (MobeelizerModelMetadata metadata in metadataToUpdate)
            {
                metadata.Modyfied = 1;
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
