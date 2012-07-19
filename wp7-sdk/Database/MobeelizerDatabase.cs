using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using System.Data.Linq;
using System.Linq;
using System;
using Microsoft.Phone.Data.Linq;
using System.Reflection;
using Com.Mobeelizer.Mobile.Wp7.Model;
using Com.Mobeelizer.Mobile.Wp7.Definition;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    public class MobeelizerDatabase : IMobeelizerDatabase
    {
        private IList<IMobeelizerTransaction> openedTransactions = new List<IMobeelizerTransaction>();

        private MobeelizerApplication application;

        private IDictionary<String, MobeelizerModel> models;

        internal String ConnectionString
        {
            get
            {
                return String.Format("DataSource=isostore:/{0}_{1}_data.sdf", this.application.InstanceGuid, this.application.User);
            }
        }

        internal string User
        {
            get
            {
                return application.User;
            }
        }

        public IMobeelizerTransaction BeginTransaction()
        {
            MobeelizerTransaction transaction = new MobeelizerTransaction(this);
            this.openedTransactions.Add(transaction);
            return transaction;
        }

        internal MobeelizerDatabaseContext BeginInternalTransaction()
        {
            return new MobeelizerDatabaseContext(this.ConnectionString);
        }

        internal void RemoveTransaction(IMobeelizerTransaction transaction)
        {
            this.openedTransactions.Remove(transaction);
        }

        internal MobeelizerDatabase(MobeelizerApplication application, IDictionary<String, MobeelizerModel> models)
        {
            this.application = application;
            this.models = models;
        }

        internal bool Exists(string modelName, string guid)
        {
            bool exists = true;
            try
            {
                using (MobeelizerDatabaseContext dataContext = new MobeelizerDatabaseContext(this.ConnectionString))
                {
                    var table = from MobeelizerWp7Model m in dataContext.GetTable(models[modelName].Type) where m.guid == guid select m;
                    table.Single();
                }
            }
            catch(SystemException)
            {
                exists = false;
            }

            return exists;
        }

        internal void Open()
        {
            using (var transaction = this.BeginInternalTransaction())
            {
                if (!transaction.DatabaseExists())
                {
                    transaction.CreateDatabase();

                    DatabaseSchemaUpdater schemaUpdater = transaction.CreateDatabaseSchemaUpdater();
                    foreach (var model in models.Values)
                    {
                        MethodInfo method = schemaUpdater.GetType().GetMethod("AddTable");
                        MethodInfo generic = method.MakeGenericMethod(model.Type);
                        generic.Invoke(schemaUpdater, null);
                    }
                    schemaUpdater.Execute();
                }
            }
        }

        internal void Close()
        {
            foreach (var transaction in openedTransactions)
            {
                transaction.Close();
            }
        }

        internal void LockModifiedFlag()
        {
            using (var transaction = this.BeginInternalTransaction())
            {
                var metadata = from model in transaction.ModelMetadata where model.Modyfied == 1 select model;
                foreach (var model in metadata)
                {
                    model.Modyfied = 2;
                }

                var files = from file in transaction.Files where file.Modyfied == 1 select file;
                foreach (var file in files)
                {
                    file.Modyfied = 2;
                }

                transaction.SubmitChanges();
            }
        }

        internal void UnlockModifiedFlag()
        {
            using (var transaction = this.BeginInternalTransaction())
            {
                var metadata = from model in transaction.ModelMetadata where model.Modyfied == 2 select model;
                foreach (var model in metadata)
                {
                    model.Modyfied = 1;
                }

                var files = from file in transaction.Files where file.Modyfied == 2 select file;
                foreach (var file in files)
                {
                    file.Modyfied = 1;
                }

                transaction.SubmitChanges();
            }
        }

        internal void ClearModifiedFlag()
        {
            using (var transaction = this.BeginInternalTransaction())
            {
                var query = from metadata in transaction.ModelMetadata where metadata.Modyfied == 2 && metadata.Deleted == 1 && metadata.Conflicted == 0 select metadata;
                foreach (var metadata in query)
                {
                    var modelQuery = from MobeelizerWp7Model model in transaction.GetTable(models[metadata.Model].Type) where model.guid == metadata.Guid select model;
                    MobeelizerWp7Model modelObject = modelQuery.Single();
                    transaction.GetTable(models[metadata.Model].Type).DeleteOnSubmit(modelObject);
                }

                var query2 = from model in transaction.ModelMetadata where model.Modyfied == 2 select model;
                foreach (var model in query2)
                {
                    model.Modyfied = 0;
                }

                var files = from file in transaction.Files where file.Modyfied == 2 select file;
                foreach (var file in files)
                {
                    file.Modyfied = 0;
                }

                transaction.SubmitChanges();
            }
        }

        internal MobeelizerSyncEnumerable GetEntitiesToSync()
        {
            return new MobeelizerSyncEnumerable(this, models); 
        }

        internal bool UpdateEntitiesFromSync(IEnumerable<MobeelizerJsonEntity> entities, bool isAllSynchronization)
        {
            bool isTransactionSuccess = true;
            using (MobeelizerDatabaseContext dataContext = new MobeelizerDatabaseContext(this.ConnectionString))
            {
                if (isAllSynchronization)
                {
                    foreach (var model in models.Values)
                    {
                        var table = dataContext.GetTable(model.Type);
                        table.DeleteAllOnSubmit(from MobeelizerWp7Model record in table select record);
                    }

                    dataContext.ModelMetadata.DeleteAllOnSubmit(from m in dataContext.ModelMetadata select m);
                    //dataContext.Files.DeleteAllOnSubmit(from f in dataContext.Files select f);
                    dataContext.SubmitChanges();
                }

                foreach (MobeelizerJsonEntity entity in entities)
                {
                    isTransactionSuccess = models[entity.Model].UpdateFromSync(entity, dataContext);
                    if (!isTransactionSuccess)
                    {
                        break;
                    }
                }

                if (isTransactionSuccess)
                {
                    dataContext.SubmitChanges();
                }
            }

            return isTransactionSuccess;
        }

        internal bool ValidateEntity(MobeelizerWp7Model insert, MobeelizerErrorsHolder errors)
        {
            String model = insert.GetType().Name;
            return models[model].Validate(insert, errors);
        }

        internal MobeelizerSyncFileEnumerable GetFilesToSync()
        {
            return new MobeelizerSyncFileEnumerable(new MobeelizerDatabaseContext(this.ConnectionString));
        }

        internal String GetFilePath(String guid)
        {
            String path = null;
            using (MobeelizerDatabaseContext transaction = new MobeelizerDatabaseContext(this.ConnectionString))
            {
                try
                {
                    var query = from f in transaction.Files where f.Guid == guid select f;
                    var result = query.Single();
                    path = result.Path;
                }
                catch { }
            }
            return path;
        }

        internal void DeleteFileFromSync(String guid)
        {
            using (MobeelizerDatabaseContext transaction = new MobeelizerDatabaseContext(this.ConnectionString))
            {
                var query = from f in transaction.Files where f.Guid == guid select f;
                transaction.Files.DeleteAllOnSubmit(query);
                transaction.SubmitChanges();
            }
        }

        internal bool IsFileExists(String guid)
        {
            bool fileExists = false;
            using (MobeelizerDatabaseContext transaction = new MobeelizerDatabaseContext(this.ConnectionString))
            {
                try
                {
                    var query = from f in transaction.Files where f.Guid == guid select f;
                    var result = query.Single();
                    if (result != null)
                    {
                        fileExists = true;
                    }
                }
                catch
                {
                    fileExists = false;
                }
            }

            return fileExists;
        }

        internal void AddFile(String guid, String path)
        {
            this.AddFile(guid, path, 1);
        }

        internal void AddFileFromSync(String guid, String path)
        {
            this.AddFile(guid, path, 0);
        }

        private void AddFile(String guid, String path, int modificationFlag)
        {
            using (MobeelizerDatabaseContext transaction = new MobeelizerDatabaseContext(this.ConnectionString))
            {
                MobeelizerFilesTableEntity entity = new MobeelizerFilesTableEntity();
                entity.Guid = guid;
                entity.Path = path;
                entity.Modyfied = modificationFlag;
                transaction.Files.InsertOnSubmit(entity);
                transaction.SubmitChanges(ConflictMode.FailOnFirstConflict);
            }
        }
    }
}
