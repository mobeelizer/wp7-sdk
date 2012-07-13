using System;
using Com.Mobeelizer.Mobile.Wp7.Definition.Types.Helpers;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public enum MobeelizerFieldType
    {
        TEXT,
        INTEGER,
        BOOLEAN,
        DECIMAL,
        DATE,
        BELONGS_TO,
        FILE
    }

    public static class MobeelizerFieldTypeExtension
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

        public static IList<Object> GetAccessibleTypes(this MobeelizerFieldType fieldType)
        {
            return null;
        }

        public static void SetValueFromMapToDatabase(this MobeelizerFieldType fieldType, IDictionary<String, object> values,  IDictionary<String, String> map, MobeelizerFieldAccessor field, bool required,  IDictionary<String, String> options, MobeelizerErrorsHolder errors) 
        {
            typesMap[fieldType].SetValueFromMapToDatabase(values, map, field, required, options, errors);
        }
    }
}
