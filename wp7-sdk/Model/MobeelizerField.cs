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
        
        private MobeelizerFieldAccessor accesor;

        internal MobeelizerField(Type type, Definition.MobeelizerModelFieldDefinition radField, Definition.MobeelizerModelFieldCredentialsDefinition fieldCredentials)
        {
            this.field = radField;
            this.fieldCredentials = fieldCredentials;
            this.Name = radField.Name;
            this.FieldType = radField.Type;
            this.accesor = new MobeelizerFieldAccessor(type, GetPropertyName(this.Name));
            PropertyInfo info = type.GetProperty(this.accesor.Name);
            if (info == null)
            {
                throw new ConfigurationException("Model '"+ type.Name + "' does not contains property '"+ this.accesor.Name+"'.");
            }

            if(!FieldType.Supports(info.PropertyType))
            {
                throw new ConfigurationException("Problem with model '" + type.Name + "', property '"+this.Name+"' type (" + info.PropertyType .Name+ ") not supported.");
            }
        }

        internal string Name { get; private set; }

        internal MobeelizerFieldAccessor Accessor
        {
            get
            {
                return this.accesor;
            }
        }

        internal MobeelizerFieldType FieldType { get; private set; }

        internal void SetValueFromMapToDatabase(IDictionary<string, object> values, IDictionary<string, string> map, MobeelizerErrorsHolder errors)
        {
            FieldType.SetValueFromMapToDatabase(values, map, accesor ,  this.field.IsRequired, field.Options ,errors);
        }

        internal void Validate(Dictionary<string, object> values, MobeelizerErrorsHolder errors)
        {
            FieldType.Validate(values, accesor, this.field.IsRequired, field.Options, errors);
        }

        private String GetPropertyName(String fieldName)
        {
            String firstOne = fieldName.Substring(0, 1);
            String tail = fieldName.Substring(1);
            return firstOne.ToUpper() + tail;
        }
    }
}
