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
using Com.Mobeelizer.Mobile.Wp7.Definition.Type.Helpers;
using System.Collections.Generic;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public enum MobeelizerFieldType
    {
        TEXT,// = new MobeelizerTextFieldTypeHelper(),
        INTEGER,// = new MobeelizerIntegerFieldTypeHelper(),
        BOOLEAN,// = new MobeelizerBooleanFieldTypeHelper(),
        DECIMAL,// = new MobeelizerDecimalFieldTypeHelper(),
        DATE,// = new MobeelizerDateFieldTypeHelper(),
        BELONGS_TO,// = new MobeelizerBelongsToFieldTypeHelper(),
        FILE,// = new MobeelizerFileFieldTypeHelper()
    }

    public static class MobeelizerFieldTypeExtension
    {
        // TODO : finish this, create dictionary with helpers

        public static IList<Object> GetAccessibleTypes(this MobeelizerFieldType fieldType)
        {
            return null;
        }
    }
}
