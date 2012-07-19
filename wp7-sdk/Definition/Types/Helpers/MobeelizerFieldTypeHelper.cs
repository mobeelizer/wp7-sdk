using System;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers
{
    internal abstract class MobeelizerFieldTypeHelper
    {
        internal void SetValueFromMapToDatabase(IDictionary<string, object> values, IDictionary<string, string> map, MobeelizerFieldAccessor field, bool required, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            String value = map[field.Name];
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
    }
}
