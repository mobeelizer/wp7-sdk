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
    public class MobeelizerBelongsToFieldTypeHelper : MobeelizerFieldTypeHelper
    {
        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            String stringValue = (String)ConvertFromEntityValueToDatabaseValue(field, value, options, errors);

            if (!errors.IsValid)
            {
                return;
            }

            if (!Mobeelizer.GetDatabase().Exists(options["model"], stringValue)) 
            {
                errors.AddFieldMissingReferenceError(field.Name, stringValue);
                return;
            }

            values.Add(field.Name, stringValue);
        }

        private object ConvertFromEntityValueToDatabaseValue(MobeelizerFieldAccessor field, string value, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            throw new NotImplementedException();
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, (String)null);
        }
    }
}
