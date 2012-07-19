using System;
using Com.Mobeelizer.Mobile.Wp7.Definition;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.Mobile.Configuration;

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
            PropertyInfo info = type.GetProperty(this.Name);
            if (info == null)
            {
                throw new ConfigurationException("Model '"+ type.Name + "' does not contains property '"+ this.Name+"'.");
            }

            if(!FieldType.Supports(info.PropertyType))
            {
                throw new ConfigurationException("Problem with model '" + type.Name + "', property '"+this.Name+"' type (" + info.PropertyType .Name+ ") not supported.");
            }
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
