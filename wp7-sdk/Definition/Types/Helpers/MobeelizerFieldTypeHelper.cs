using System;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers
{
    internal abstract class MobeelizerFieldTypeHelper
    {
        private String GetFieldName(String propertyName)
        {
            String firstOne = propertyName.Substring(0, 1);
            String tail = propertyName.Substring(1);
            return firstOne.ToLower() + tail;
        }

        internal void SetValueFromMapToDatabase(IDictionary<string, object> values, IDictionary<string, string> map, MobeelizerFieldAccessor field, bool required, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            String value = null;
            if (map.ContainsKey(GetFieldName(field.Name)))
            {
                value = map[GetFieldName(field.Name)];
            }
            else if (map.ContainsKey(field.Name))
            {
                value = map[field.Name];
            }

            if (value == null && required)
            {
                errors.AddFieldCanNotBeEmpty(field.Name);
                return;
            }

            if (value == null)
            {
                SetNullValueFromMapToDatabase(values, field, options, errors);
            }
            else
            {
                SetNotNullValueFromMapToDatabase(values, value, field, options, errors);
            }
        }

        protected virtual void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
        }

        protected virtual void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
        }

        public void Validate(IDictionary<String, object> values, MobeelizerFieldAccessor field, bool required, IDictionary<String, String> options, MobeelizerErrorsHolder errors)
        {
            Object value = values[field.Name];

            if (value == null && required)
            {
                errors.AddFieldCanNotBeEmpty(field.Name);
                return;
            }

            if (value != null)
            {
                ValidateValue(value, field, options, errors);
            }
        }

        protected virtual void ValidateValue(object value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
        }

        internal virtual bool Supports(Type type)
        {
            return false;
        }

        internal virtual string SetValueFromDatabaseToMap(object value)
        {
            return value.ToString();
        }
    }
}
