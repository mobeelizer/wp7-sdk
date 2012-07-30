using System;
using Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    internal enum MobeelizerFieldType
    {
        TEXT,
        INTEGER,
        BOOLEAN,
        DECIMAL,
        DATE,
        BELONGS_TO,
        FILE
    }

    internal static class MobeelizerFieldTypeExtension
    {
        private static Dictionary<MobeelizerFieldType, MobeelizerFieldTypeHelper> typesMap = new Dictionary<MobeelizerFieldType, MobeelizerFieldTypeHelper>()
            {
                {MobeelizerFieldType.TEXT, new MobeelizerTextFieldTypeHelper()},
                {MobeelizerFieldType.INTEGER, new MobeelizerIntegerFieldTypeHelper()},
                {MobeelizerFieldType.BOOLEAN, new MobeelizerBooleanFieldTypeHelper()},
                {MobeelizerFieldType.DECIMAL, new MobeelizerDecimalFieldTypeHelper()},
                {MobeelizerFieldType.DATE, new MobeelizerDateFieldTypeHelper()},
                {MobeelizerFieldType.BELONGS_TO, new MobeelizerBelongsToFieldTypeHelper()},
                {MobeelizerFieldType.FILE, new MobeelizerFileFieldTypeHelper()}
            };


        internal static void SetValueFromMapToDatabase(this MobeelizerFieldType fieldType, IDictionary<String, object> values, IDictionary<String, String> map, MobeelizerFieldAccessor field, bool required, IDictionary<String, String> options, MobeelizerErrorsHolder errors) 
        {
            typesMap[fieldType].SetValueFromMapToDatabase(values, map, field, required, options, errors);
        }

        internal static void Validate(this MobeelizerFieldType fieldType, IDictionary<String, object> values, MobeelizerFieldAccessor field, bool required, IDictionary<String, String> options, MobeelizerErrorsHolder errors)
        {
            typesMap[fieldType].Validate(values,field, required, options, errors);
        }

        internal static bool Supports(this MobeelizerFieldType fieldType, Type type)
        {
            return typesMap[fieldType].Supports(type);
        }

        internal static String SetValueFromDatabaseToMap(this MobeelizerFieldType fieldType, object value)
        {
            return typesMap[fieldType].SetValueFromDatabaseToMap(value);
        }
    }
}
