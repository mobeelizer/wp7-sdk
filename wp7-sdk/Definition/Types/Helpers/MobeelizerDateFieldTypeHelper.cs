using System;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers
{
    internal class MobeelizerDateFieldTypeHelper : MobeelizerFieldTypeHelper
    {
        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            DateTime date = new DateTime(TimeSpan.FromMilliseconds(Int64.Parse(value)).Ticks + new DateTime(1970,1,1,2,0,0,DateTimeKind.Utc).Ticks); 
            values.Add(field.Name, date);
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, null);
        }

        internal override bool Supports(Type type)
        {
            if (type == typeof(DateTime?))
            {
                return true;
            }

            return false;
        }

        internal override string SetValueFromDatabaseToMap(object value)
        {
            if (value == null)
            {
                return null;
            }

            DateTime? dateTime = value as DateTime?;
            return ((long)((dateTime.Value - new DateTime(1970, 1, 1, 2,0,0,DateTimeKind.Utc)).TotalMilliseconds)).ToString();
        }
    }
}
