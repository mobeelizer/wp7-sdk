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
    internal class MobeelizerTextFieldTypeHelper : MobeelizerFieldTypeHelper
    {
        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, value);
        }

        private bool ValidateValue(MobeelizerFieldAccessor field, string value, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            int maxLength = GetMaxLength(options);

            if (((String)value).Length > maxLength)
            {
                errors.AddFieldIsTooLong(field.Name, maxLength);
                return false;
            }

            return true;
        }

        private int GetMaxLength(IDictionary<string, string> options)
        {
            return options.ContainsKey("maxLength") ? Int32.Parse(options["maxLength"]) : 4096;
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, null);
        }

        protected override void ValidateValue(object value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            ValidateValue(field, (String)value, options, errors);
        }

        internal override bool Supports(Type type)
        {
            if (type == typeof(String))
            {
                return true;
            }

            return false;
        }
    }
}
