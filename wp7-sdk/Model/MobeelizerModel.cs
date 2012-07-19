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
            entity.Owner = metadata.Owner;
            entity.Fields = new Dictionary<string, string>();
            entity.Fields.Add("s_deleted", (metadata.Deleted == 0) ? "false" : "true");
            var result = from MobeelizerWp7Model model in db.GetTable(this.Type) where model.guid == metadata.Guid select model;
            MobeelizerWp7Model modelObject = result.Single();
            foreach (MobeelizerField field in this.Fields)
            {
                object value = null;
                PropertyInfo property = modelObject.GetType().GetProperty(field.Name);
                if (property == null)
                {
                    throw new ConfigurationException("There is no property " + field.Name + " in " + entity.Model + " class.");
                }

                value = property.GetValue(modelObject, null);
                entity.Fields.Add(field.Name, value.ToString());
            }

            return entity;
        }

        internal bool UpdateFromSync(MobeelizerJsonEntity entity, MobeelizerDatabaseContext db)
        {
            var query = from MobeelizerModelMetadata m in db.ModelMetadata where m.Guid == entity.Guid && m.Model == this.Name select m;
            bool exists = true;
            MobeelizerModelMetadata metadata = null;
            if (query.Count() == 0)
            {
                exists = false;
            }
            else
            {
                metadata = query.Single();
            }

            
            bool modifiedByUser = exists && metadata.Modyfied == 1;

            if (modifiedByUser || !exists && entity.IsDeleted)
            {
                return true;
            }

            if (entity.ConflictState == MobeelizerJsonEntity.MobeelizerConflictState.NO_IN_CONFLICT && entity.IsDeleted)
            {
                if (exists)
                {
                    var table = db.GetTable(this.Type);
                    table.DeleteAllOnSubmit(from MobeelizerWp7Model record in table where record.guid == entity.Guid select record);
                }

                return true;
            }

            Dictionary<String, object> values = new Dictionary<string, object>();
            if (entity.ConflictState == MobeelizerJsonEntity.MobeelizerConflictState.IN_CONFLICT_BECAUSE_OF_YOU || entity.Fields == null)
            {
                metadata.Conflicted = 1;
                metadata.Modyfied = 0;
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
            values.Add("Modyfied", 0);
            values.Add("Deleted", entity.IsDeleted ? 1 : 0);
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
                UpdateEntity(db, metadata, values, entity.Guid);
            }
            else
            {
                values.Add("guid", entity.Guid);
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
                if (value.Key == "guid")
                {
                    metadate.Guid = (String)value.Value;
                    PropertyInfo property = this.Type.GetProperty("guid");
                    property.SetValue(entity, value.Value, null);
                }
                if (value.Key == "Conflicted")
                {
                    metadate.Conflicted = (Int32)value.Value;
                }
                else if (value.Key == "Modyfied")
                {
                    metadate.Modyfied = (Int32)value.Value;
                }
                else if (value.Key == "Deleted")
                {
                    metadate.Deleted = (Int32)value.Value;
                }
                else if (value.Key == "Owner")
                {
                    metadate.Owner = (String)value.Value;
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

        private void UpdateEntity(MobeelizerDatabaseContext db, MobeelizerModelMetadata metadate, IDictionary<String, object> values, String guid)
        {
            var query = from MobeelizerWp7Model m in db.GetTable(this.Type) where m.guid == guid select m;
            MobeelizerWp7Model entity = query.Single();
            foreach (KeyValuePair<String, object> value in values)
            {
                if (value.Key == "Conflicted")
                {
                    metadate.Conflicted = (Int32)value.Value;
                }
                else if (value.Key == "Modyfied")
                {
                    metadate.Modyfied = (Int32)value.Value;
                }
                else if (value.Key == "Deleted")
                {
                    metadate.Deleted = (Int32)value.Value;
                }
                else if (value.Key == "Owner")
                {
                    metadate.Owner = (String)value.Value;
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
                PropertyInfo info = type.GetProperty(filed.Name);
                values.Add(info.Name, info.GetValue(model,null));
            }
        }
    }
}
