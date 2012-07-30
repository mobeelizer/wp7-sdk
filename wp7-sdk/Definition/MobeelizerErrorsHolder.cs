using System;
using System.Collections.Generic;
using System.Text;
using Com.Mobeelizer.Mobile.Wp7.Database;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    internal class MobeelizerErrorsHolder
    {
        private Dictionary<String, List<MobeelizerError>> errors = new Dictionary<String, List<MobeelizerError>>();

        internal bool IsValid
        {
            get
            {
                return errors.Count == 0;
            }
        }

        internal void AddFieldCanNotBeEmpty(string field)
        {
            AddError(field, MobeelizerErrorCode.EMPTY, MobeelizerErrorCode.EMPTY.GetMessage(), null);
        }

        internal void AddFieldMissingReferenceError(string field, string guid)
        {
            AddError(field, MobeelizerErrorCode.NOT_FOUND, MobeelizerErrorCode.NOT_FOUND.GetMessage(), guid);
        }

        internal void AddFieldMustBeLessThanOrEqualTo(string p, double maxValue)
        {
            AddError(p, MobeelizerErrorCode.LESS_THAN_OR_EQUAL_TO, MobeelizerErrorCode.LESS_THAN_OR_EQUAL_TO.GetMessage(),maxValue);
        }

        internal void AddFieldMustBeLessThan(string p, double maxValue)
        {
            AddError(p, MobeelizerErrorCode.LESS_THAN, MobeelizerErrorCode.LESS_THAN.GetMessage(), maxValue);
        }

        internal void AddFieldMustBeGreaterThanOrEqual(string p, double minValue)
        {
            AddError(p, MobeelizerErrorCode.GREATER_THAN_OR_EQUAL_TO, MobeelizerErrorCode.GREATER_THAN_OR_EQUAL_TO.GetMessage(), minValue);
        }

        internal void AddFieldMustBeGreaterThan(string p, double minValue)
        {
            AddError(p, MobeelizerErrorCode.GREATER_THAN, MobeelizerErrorCode.GREATER_THAN.GetMessage(), minValue);
        }

        internal void AddFieldIsTooLong(string p, int maxLength)
        {
            AddError(p, MobeelizerErrorCode.TOO_LONG, MobeelizerErrorCode.TOO_LONG.GetMessage(), maxLength);
        }

        internal String GetErrorsSymmary()
        {
            StringBuilder buidler = new StringBuilder();
            buidler.AppendLine("Validation errors");
            foreach(var fieldErrors in errors)
            {
                buidler.Append("\tField '").Append(fieldErrors.Key).AppendLine("' not valid:");
                foreach (MobeelizerError message in fieldErrors.Value)
                {
                    buidler.Append("\t\t").AppendLine(message.Message);
                }
            }

            return buidler.ToString();
        }

        private void AddError(String field, MobeelizerErrorCode code, String message, object args)
        {
            if (!errors.ContainsKey(field))
            {
                errors.Add(field, new List<MobeelizerError>());
            }

            errors[field].Add(new MobeelizerError(code, message, args));
        }
    }
}
