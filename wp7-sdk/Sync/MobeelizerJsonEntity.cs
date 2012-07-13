using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Com.Mobeelizer.Mobile.Wp7.Sync
{
    public class MobeelizerJsonEntity
    {
        public enum MobeelizerConflictState
        {
            NO_IN_CONFLICT,
            IN_CONFLICT_BECAUSE_OF_YOU,
            IN_CONFLICT
        }

        private Dictionary<String, String> fields;

        private String model;

        private String guid;

        private MobeelizerConflictState conflictState;

        private String owner;

        public MobeelizerJsonEntity()
        {

        }

        public MobeelizerJsonEntity(String json)
        {
            JObject jsonObject = JObject.Parse(json);
            model = (String)jsonObject["model"];
            guid = (String)jsonObject["guid"];

            try
            {
                owner = jsonObject["owner"].ToString();
            }
            catch (KeyNotFoundException) { }

            try
            {
                conflictState = (MobeelizerConflictState)(Enum.Parse(typeof(MobeelizerConflictState), jsonObject["conflictState"].ToString(), true));
            }
            catch (KeyNotFoundException)
            {
                conflictState = MobeelizerConflictState.NO_IN_CONFLICT;
            }

            try
            {
                JObject jsonFields = (JObject)jsonObject["fields"];
                fields = new Dictionary<String, String>();
                foreach (KeyValuePair<String, JToken> key in jsonFields)
                {
                    fields.Add(key.Key, key.Value.ToString());
                }
            }
            catch (KeyNotFoundException) { }
        }

        public String Model
        {
            get
            {
                return this.model;
            }
            set
            {
                this.model = value;
            }
        }

        public String Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.guid = value;
            }
        }

        public String Owner
        {
            get
            {
                return owner;
            }
            set
            {
                this.owner = value;
            }
        }

        public Dictionary<String, String> Fields
        {
            get
            {
                return fields;
            }
            set
            {
                this.fields = value;
            }
        }

        public bool ContainsValue(String field)
        {
            return fields == null ? false : fields.ContainsKey(field);
        }

        public String GetValue(String field)
        {
            return fields == null ? null : fields[field];
        }

        public MobeelizerConflictState ConflictState
        {
            get
            {
                return conflictState;
            }
            set
            {
                this.conflictState = value;
            }
        }

        public bool IsDeleted
        {
            get
            {
                String deleted = GetValue("s_deleted");

                if (deleted == null)
                {
                    throw new InvalidOperationException("Cannot find s_deleted field in " + Guid + ": " + fields);
                }

                if ("true".Equals(deleted))
                {
                    return true;
                }
                else if ("false".Equals(deleted))
                {
                    return false;
                }

                throw new InvalidOperationException("Illegal value '" + deleted + "' of s_deleted field in " + Guid);
            }
        }

        public String GetJson()
        {
            JObject json = new JObject();
            json.Add("model", model);
            json.Add("guid", guid);
            json.Add("resolveConflict", "false");
            json.Add("owner", owner);
            json.Add("conflictState", conflictState.ToString());

            if (fields != null)
            {
                JObject jsonFields = new JObject();

                foreach (KeyValuePair<String, String> field in fields)
                {
                    jsonFields.Add(field.Key, field.Value);
                }

                json.Add("fields", jsonFields);
            }

            return json.ToString().Replace("\r\n","");
        }

        public override String ToString()
        {
            return GetJson();
        }
    }
}
