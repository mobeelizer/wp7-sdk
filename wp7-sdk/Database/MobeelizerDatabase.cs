using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using System.Data.Linq;
using System.Linq;
using System;
using Microsoft.Phone.Data.Linq;
using System.Reflection;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    public class MobeelizerDatabase : IMobeelizerDatabase
    {
        private MobeelizerDatabaseContext dataContext;

        private MobeelizerApplication application;

        private IDictionary<String, MobeelizerModel> models;

        public MobeelizerDatabase(MobeelizerApplication application, IDictionary<String, MobeelizerModel> models)
        {
            this.application = application;
            this.models = models;
        }


        public bool Exists(string modelName, string guid)
        {
            bool exists = true;
            try
            {
                var table = from MobeelizerWp7Model m in this.dataContext.GetTable(models[modelName].Type) where m.guid == guid select m;
                table.Single();
            }
            catch(SystemException)
            {
                exists = false;
            }

            return exists;
        }

        internal void Open()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
                dataContext = null;
            }

            dataContext = new MobeelizerDatabaseContext(application.InstanceGuid, application.User);
            
            if (!dataContext.DatabaseExists())
            {
                dataContext.CreateDatabase();

                DatabaseSchemaUpdater schemaUpdater = dataContext.CreateDatabaseSchemaUpdater();
                foreach (var model in models.Values)
                {
                    MethodInfo method = schemaUpdater.GetType().GetMethod("AddTable");
                    MethodInfo generic = method.MakeGenericMethod(model.Type);
                    generic.Invoke(schemaUpdater, null);
                }
                schemaUpdater.Execute();
            }
        }

        public void AddTable<T>()
        {
            DatabaseSchemaUpdater schemaUpdater = dataContext.CreateDatabaseSchemaUpdater();
            schemaUpdater.AddTable<T>();
            schemaUpdater.Execute();
        }

        internal void Close()
        {
            if (dataContext != null)
            {
                dataContext.Dispose();
                dataContext = null;
            }
        }

        internal void LockModifiedFlag()
        {
            var metadata = from model in dataContext.ModelMetadata where model.Modyfied == 1 select model;
            foreach (var model in metadata)
            {
                model.Modyfied = 2;
            }

            dataContext.SubmitChanges();
        }

        internal void UnlockModifiedFlag()
        {
            var metadata = from model in dataContext.ModelMetadata where model.Modyfied == 2 select model;
            foreach (var model in metadata)
            {
                model.Modyfied = 1;
            }

            dataContext.SubmitChanges();
        }

        internal void ClearModifiedFlag()
        {
            var query = from metadata in dataContext.ModelMetadata where metadata.Modyfied == 2 && metadata.Deleted == 1 && metadata.Conflicted == 0 select metadata;
            foreach (var metadata in query)
            {
                var modelQuery = from MobeelizerWp7Model model in dataContext.GetTable(models[metadata.Model].Type) where model.guid == metadata.Guid select model;
                MobeelizerWp7Model modelObject = modelQuery.Single();
                dataContext.GetTable(models[metadata.Model].Type).DeleteOnSubmit(modelObject);
            }

            var query2 = from model in dataContext.ModelMetadata where model.Modyfied == 2 select model;
            foreach (var model in query2)
            {
                model.Modyfied = 0;
            }

            dataContext.SubmitChanges();
        }

        public MobeelizerSyncEnumerable GetEntitiesToSync()
        {
            return new MobeelizerSyncEnumerable(this.dataContext, models); 
        }

        public MobeelizerSyncFileIterator GetFilesToSync()
        {
            return new MobeelizerSyncFileIterator(); // TODO
        }


        public bool UpdateEntitiesFromSync(IEnumerable<MobeelizerJsonEntity> entities, bool isAllSynchronization)
        {
            bool isTransactionSuccess = true;
            if (isAllSynchronization)
            {
                foreach (var model in models.Values)
                {
                    var table = dataContext.GetTable(model.Type);
                    table.DeleteAllOnSubmit(from MobeelizerWp7Model record in table select record);
                }

                dataContext.ModelMetadata.DeleteAllOnSubmit(from m in dataContext.ModelMetadata select m);
                try
                {
                    dataContext.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch 
                {
                    isTransactionSuccess = false;
                }
            }

            foreach (MobeelizerJsonEntity entity in entities)
            {
                isTransactionSuccess = models[entity.Model].UpdateFromSync(entity, this.dataContext);
                if (!isTransactionSuccess)
                {
                    break;
                }
            }
            if (isTransactionSuccess)
            {
                try
                {
                    this.dataContext.SubmitChanges(ConflictMode.FailOnFirstConflict);
                }
                catch
                {
                    isTransactionSuccess = false;
                }
            }

            return isTransactionSuccess;
        }


        public ITable<T> GetModels<T>() where T : MobeelizerWp7Model
        {
            return dataContext.GetModels<T>();
        }


        public void Commit()
        {
            // TODO : Sprawdzanie ograniczen na pola.
            ChangeSet set = dataContext.GetChangeSet();
            IList<MobeelizerModelMetadata> metadataToAdd = new List<MobeelizerModelMetadata>();
            IList<MobeelizerModelMetadata> metadataToUpdate = new List<MobeelizerModelMetadata>();
            
            foreach (var insert in set.Inserts)
            {
                if (insert is MobeelizerWp7Model)
                {
                    String model = insert.GetType().Name;
                    String guid = Guid.NewGuid().ToString();
                    (insert as MobeelizerWp7Model).guid = guid;
                    String owner = application.User;
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
    }
}
