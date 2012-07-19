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
    internal class MobeelizerIntegerFieldTypeHelper : MobeelizerFieldTypeHelper
    {

        protected override void SetNotNullValueFromMapToDatabase(IDictionary<string, object> values, string value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            Int32 intValue = Int32.Parse(value);

            values.Add(field.Name, intValue);
        }

        private bool ValidateValue(MobeelizerFieldAccessor field, long longValue, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            int maxValue = GetMaxValue(options);
            int minValue = GetMinValue(options);

            if (longValue > maxValue)
            {
                errors.AddFieldMustBeLessThan(field.Name, (long)maxValue);
                return false;
            }

            if (longValue < minValue)
            {
                errors.AddFieldMustBeGreaterThan(field.Name, (long)minValue);
                return false;
            }

            return true;
        }

        private int GetMaxValue(IDictionary<string, string> options)
        {
            return options.ContainsKey("maxValue") ? Int32.Parse(options["maxValue"]): Int32.MaxValue;
        }

        private int GetMinValue(IDictionary<string, string> options)
        {
            return options.ContainsKey("minValue") ? Int32.Parse(options["minValue"]) : Int32.MinValue;
        }

        protected override void SetNullValueFromMapToDatabase(IDictionary<string, object> values, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            values.Add(field.Name, null);
        }

        protected override void ValidateValue(object value, MobeelizerFieldAccessor field, IDictionary<string, string> options, MobeelizerErrorsHolder errors)
        {
            ValidateValue(field, (int)value, options, errors);
        }

        internal override bool Supports(Type type)
        {
            if (type == typeof(int))
            {
                return true;
            }

            return false;
        }
    }
}
