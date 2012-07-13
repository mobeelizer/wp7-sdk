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
    public class MobeelizerDecimalFieldTypeHelper : MobeelizerFieldTypeHelper
    {

        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            Double doubleValue = (Double)ConvertFromEntityValueToDatabaseValue(field, value, options, errors);

            if (!errors.IsValid)
            {
                return;
            }

            values.Add(field.Name, doubleValue);
        }

        private object ConvertFromEntityValueToDatabaseValue(MobeelizerFieldAccessor field, string value, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            Double doubleValue = Double.Parse(value);

            if (!ValidateValue(field, doubleValue, options, errors))
            {
                return null;
            }

            return doubleValue;
        }

        protected bool GetIncludeMaxValue(IDictionary<String, String> options)
        {
            try
            {
                return Boolean.Parse(options["includeMaxValue"]);
            }
            catch
            {
                return false;
            }
        }

        protected bool GetIncludeMinValue(IDictionary<String, String> options)
        {
            try
            {
                return Boolean.Parse(options["includeMinValue"]);
            }
            catch
            {
                return false;
            }
        }

        protected String GetMinValue(IDictionary<String, String> options)
        {
            try
            {
                return options["minValue"];
            }
            catch
            {
                return "0";
            }
        }

        protected String GetMaxValue(IDictionary<String, String> options)
        {
            try
            {
                return options["maxValue"];
            }
            catch
            {
                return "0";
            }
        }

        private bool ValidateValue(MobeelizerFieldAccessor field, double doubleValue, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            bool includeMaxValue = GetIncludeMaxValue(options);
            bool includeMinValue = GetIncludeMinValue(options);
            Double minValue = Double.Parse(GetMinValue(options));
            Double maxValue = Double.Parse(GetMaxValue(options));

            if (includeMaxValue && doubleValue > maxValue)
            {
                errors.AddFieldMustBeLessThanOrEqualTo(field.Name, maxValue);
                return false;
            }

            if (!includeMaxValue && doubleValue >= maxValue)
            {
                errors.AddFieldMustBeLessThan(field.Name, maxValue);
                return false;
            }

            if (includeMinValue && doubleValue < minValue)
            {
                errors.AddFieldMustBeGreaterThanOrEqual(field.Name, minValue);
                return false;
            }

            if (!includeMinValue && doubleValue <= minValue)
            {
                errors.AddFieldMustBeGreaterThan(field.Name, minValue);
                return false;
            }

            return true;
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
        }
    }
}
