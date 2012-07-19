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
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers
{
    internal class MobeelizerBelongsToFieldTypeHelper : MobeelizerFieldTypeHelper
    {
        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, String value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            String stringValue = value;
            values.Add(field.Name, stringValue);
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, (String)null);
        }


        protected override void ValidateValue(object value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            String stringValue = (String)value;

            if (!errors.IsValid)
            {
                return;
            }

            if (!((MobeelizerDatabase)Mobeelizer.GetDatabase()).Exists(options["model"], stringValue))
            {
                errors.AddFieldMissingReferenceError(field.Name, stringValue);
                return;
            }
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
