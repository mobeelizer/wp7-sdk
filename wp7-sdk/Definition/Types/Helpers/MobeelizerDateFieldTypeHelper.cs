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
    public class MobeelizerDateFieldTypeHelper : MobeelizerFieldTypeHelper
    {

        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            DateTime date = ConvertFromEntityValueToDatabaseValue(field, value, options, errors);

            if (!errors.IsValid)
            {
                return;
            }

            values.Add(field.Name, date);
        }

        private DateTime ConvertFromEntityValueToDatabaseValue(MobeelizerFieldAccessor field, string value, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            //TODO check how is it represented in json entity
            throw new NotImplementedException();
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, null);
        }
    }
}
