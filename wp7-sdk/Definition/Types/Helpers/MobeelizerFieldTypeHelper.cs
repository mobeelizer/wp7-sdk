using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers
{
    public abstract class MobeelizerFieldTypeHelper
    {
        internal System.Collections.Generic.IList<object> GetAccessibleTypes()
        {
            throw new NotImplementedException();
        }

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
    }
}
