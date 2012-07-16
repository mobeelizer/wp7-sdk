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
using Com.Mobeelizer.Mobile.Wp7.Definition;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Model
{
    internal class MobeelizerField
    {
        private MobeelizerModelFieldDefinition field;
        
        private MobeelizerModelFieldCredentialsDefinition fieldCredentials;
        
        MobeelizerFieldAccessor accesor;

        internal MobeelizerField(Type type, Definition.MobeelizerModelFieldDefinition radField, Definition.MobeelizerModelFieldCredentialsDefinition fieldCredentials)
        {
            this.field = radField;
            this.fieldCredentials = fieldCredentials;
            this.Name = radField.Name;
            this.FieldType = radField.Type;
            this.accesor = new MobeelizerFieldAccessor(type, radField.Name);
        }

        internal string Name { get; private set; }

        internal MobeelizerFieldType FieldType { get; private set; }

        internal void SetValueFromMapToDatabase(IDictionary<string, object> values, IDictionary<string, string> map, MobeelizerErrorsHolder errors)
        {
            FieldType.SetValueFromMapToDatabase(values, map, accesor ,  this.field.IsRequired, field.Options ,errors);
        }

        internal void Validate(Dictionary<string, object> values, MobeelizerErrorsHolder errors)
        {
            FieldType.Validate(values, accesor, this.field.IsRequired, field.Options, errors);
        }
    }
}
