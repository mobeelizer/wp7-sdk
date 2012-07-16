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
    internal class MobeelizerBooleanFieldTypeHelper : MobeelizerFieldTypeHelper
    {

        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            bool boolValue = (bool)ConvertFromEntityValueToDatabaseValue(field, value, options, errors);

            if (!errors.IsValid)
            {
                return;
            }

            values.Add(field.Name, boolValue);
        }

        private bool ConvertFromEntityValueToDatabaseValue(MobeelizerFieldAccessor field, string value, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            try
            {
                return Boolean.Parse(value);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, null);
        }
    }
}
