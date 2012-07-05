using System;
using Com.Mobeelizer.Mobile.Wp7.Definition.Type.Helpers;
using System.Collections.Generic;

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
        private static Dictionary<MobeelizerFieldType, MobeelizerFieldTypeHelper> map = new Dictionary<MobeelizerFieldType, MobeelizerFieldTypeHelper>()
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
    }
}
