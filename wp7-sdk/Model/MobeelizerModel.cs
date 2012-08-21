using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Com.Mobeelizer.Mobile.Wp7.Database;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using Com.Mobeelizer.Mobile.Wp7.Sync;
using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7.Model
{
    internal class MobeelizerModel 
    {
        public MobeelizerModelCredentialsDefinition Credentials { get; private set; }

        public IList<MobeelizerField> Fields { get; private set; }

        public Type Type { get; private set; }

        public string Name { get; set; }

        public MobeelizerModel(Type type, string name, Definition.MobeelizerModelCredentialsDefinition credentials, IList<MobeelizerField> fields)
        {
            this.Type = type;
            this.Name = name;
            this.Credentials = credentials;
            this.Fields = fields;
        }

        public MobeelizerJsonEntity GetJsonEntity(MobeelizerModelMetadata metadata, MobeelizerDatabaseContext db)
        {
            MobeelizerJsonEntity entity = new MobeelizerJsonEntity();
            entity.Model = metadata.Model;
            entity.Guid = metadata.Guid;
            entity.Fields = new Dictionary<string, string>();
            var result = from MobeelizerWp7Model model in db.GetTable(this.Type) where model.Guid == metadata.Guid select model;
            MobeelizerWp7Model modelObject = result.Single();
            entity.Owner = modelObject.Owner;
            entity.Fields.Add("s_deleted", modelObject.Deleted.ToString().ToLower());
            foreach (MobeelizerField field in this.Fields)
            {
                object value = null;
                PropertyInfo property = modelObject.GetType().GetProperty(field.Accessor.Name);
                if (property == null)
                {
                    throw new ConfigurationException("There is no property " + field.Accessor.Name + " in " + entity.Model + " class.");
                }

                value = property.GetValue(modelObject, null);
                entity.Fields.Add(field.Name, field.FieldType.SetValueFromDatabaseToMap(value));
            }

            return entity;
        }

        private class QueryResult
        {
            public MobeelizerWp7Model Entity { get; set; }

            public MobeelizerModelMetadata Metadata { get; set; }
        }

        internal bool UpdateFromSync(MobeelizerJsonEntity entity, MobeelizerDatabaseContext db)
        {
            var query = from MobeelizerWp7Model e in db.GetTable(this.Type) join MobeelizerModelMetadata m in db.ModelMetadata on e.Guid equals m.Guid where m.Guid == entity.Guid && m.Model == this.Name select new QueryResult() { Entity = e, Metadata = m };
            bool exists = true;
            QueryResult result = null;
            if (query.Count() == 0)
            {
                exists = false;
            }
            else
            {
                result = query.Single();
            }

            
            bool modifiedByUser = exists && result.Metadata.ModificationLock == false && result.Entity.Modified ;

            if (modifiedByUser || !exists && entity.IsDeleted)
            {
                return true;
            }

            if (entity.ConflictState == MobeelizerJsonEntity.MobeelizerConflictState.NO_IN_CONFLICT && entity.IsDeleted)
            {
                if (exists)
                {
                    var table = db.GetTable(this.Type);
                    table.DeleteAllOnSubmit(from MobeelizerWp7Model record in table where record.Guid == entity.Guid select record);
                }

                return true;
            }

            Dictionary<String, object> values = new Dictionary<string, object>();
            if (entity.ConflictState == MobeelizerJsonEntity.MobeelizerConflictState.IN_CONFLICT_BECAUSE_OF_YOU || entity.Fields.Count == 0)
            {
                PropertyInfo property = this.Type.GetProperty("Conflicted");
                PropertyInfo modifiedProperty = this.Type.GetProperty("Modified");
                property.SetValue(result.Entity, true, null);
                modifiedProperty.SetValue(result.Entity, false, null);
                return true;
            }
            else if (entity.ConflictState == MobeelizerJsonEntity.MobeelizerConflictState.IN_CONFLICT)
            {
                values.Add("Conflicted", 1);
            }
            else
            {
                values.Add("Conflicted", 0);
            }

            values.Add("Owner", entity.Owner);
            values.Add("Modified", 0);
            try
            {
                values.Add("Deleted", entity.IsDeleted ? 1 : 0);
            }
            catch (KeyNotFoundException)
            {
                values.Add("Deleted", false);
            }

            MobeelizerErrorsHolder errors = new MobeelizerErrorsHolder();
            foreach (MobeelizerField field in this.Fields)
            {
                field.SetValueFromMapToDatabase(values, entity.Fields, errors);
            }

            if (!errors.IsValid)
            {
                return false;
            }

            if (exists)
            {
                UpdateEntity(db, result.Metadata, values, entity.Guid, result.Entity);
            }
            else
            {
                values.Add("Guid", entity.Guid);
                InsertEntity(db, values);
            }

            return true;
        }

        private void InsertEntity(MobeelizerDatabaseContext db, IDictionary<String, object> values)
        {
            MobeelizerModelMetadata metadate = new MobeelizerModelMetadata();
            metadate.Model = this.Name;
            var entity = Activator.CreateInstance(this.Type);
            foreach (KeyValuePair<String, object> value in values)
            {
                if (value.Key == "Guid")
                {
                    metadate.Guid = (String)value.Value;
                    PropertyInfo property = this.Type.GetProperty("Guid");
                    property.SetValue(entity, value.Value, null);
                }
                if (value.Key == "Conflicted" || value.Key == "Deleted" || value.Key == "Modified")
                {
                    PropertyInfo property = this.Type.GetProperty(value.Key);
                    property.SetValue(entity, ((Int32)value.Value == 1) ? true : false, null);
                }
                else
                {
                    PropertyInfo property = this.Type.GetProperty(value.Key);
                    property.SetValue(entity, value.Value, null);
                }
            }

            db.GetTable(this.Type).InsertOnSubmit(entity);
            db.ModelMetadata.InsertOnSubmit(metadate);
            Log.i("mobeelizermodel", "Add entity from sync " + metadate.Model + ", guid: "+ metadate.Guid);
        }

        private void UpdateEntity(MobeelizerDatabaseContext db, MobeelizerModelMetadata metadate, IDictionary<String, object> values, String guid, MobeelizerWp7Model entity)
        {
            foreach (KeyValuePair<String, object> value in values)
            {
                if (value.Key == "Conflicted" || value.Key == "Deleted" || value.Key == "Modified")
                {
                    PropertyInfo property = this.Type.GetProperty(value.Key);
                    property.SetValue(entity, ((Int32)value.Value == 1) ? true : false, null);
                }
                else
                {
                    PropertyInfo property = this.Type.GetProperty(value.Key);
                    property.SetValue(entity, value.Value, null);
                }
            }
            Log.i("mobeelizermodel", "Upadate entity from sync " + metadate.Model + ", guid: " + metadate.Guid);
        }

        internal bool Validate(MobeelizerWp7Model insert, MobeelizerErrorsHolder errors)
        {
            Dictionary<String, object> values = new Dictionary<string, object>();
            MapEntityToDictionary(insert, values);
            foreach (MobeelizerField field in this.Fields)
            {
                field.Validate(values, errors);
            }

            if (!errors.IsValid)
            {
                return false;
            }

            return true;
        }

        private void MapEntityToDictionary(MobeelizerWp7Model model, Dictionary<String, object> values)
        {
            Type type = model.GetType();
            foreach (var filed in Fields)
            {
                PropertyInfo info = type.GetProperty(filed.Accessor.Name);
                values.Add(info.Name, info.GetValue(model,null));
            }
        }
    }
}
