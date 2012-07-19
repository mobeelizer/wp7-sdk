using System;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers
{
    internal class MobeelizerDecimalFieldTypeHelper : MobeelizerFieldTypeHelper
    {
        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            Double doubleValue = Double.Parse(value);
            values.Add(field.Name, doubleValue);
        }

        private bool GetIncludeMaxValue(IDictionary<String, String> options)
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

        private bool GetIncludeMinValue(IDictionary<String, String> options)
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

        private String GetMinValue(IDictionary<String, String> options)
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

        private String GetMaxValue(IDictionary<String, String> options)
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

        protected override void ValidateValue(object value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            ValidateValue(field, (Double)value, options, errors);
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

            if (!includeMaxValue && doubleValue >= Double.MaxValue)
            {
                errors.AddFieldMustBeLessThan(field.Name, maxValue);
                return false;
            }

            if (includeMinValue && doubleValue < minValue)
            {
                errors.AddFieldMustBeGreaterThanOrEqual(field.Name, minValue);
                return false;
            }

            if (!includeMinValue && doubleValue <= Double.MinValue)
            {
                errors.AddFieldMustBeGreaterThan(field.Name, minValue);
                return false;
            }

            return true;
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, null);
        }


        internal override bool Supports(Type type)
        {
            if (type == typeof(double))
            {
                return true;
            }

            return false;
        }
    }
}
