using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    internal class MobeelizerDefinitionParser
    {
        private MobeelizerDefinitionParser()
        {
        }

        private static MobeelizerDefinitionParser instance;

        private static MobeelizerDefinitionParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MobeelizerDefinitionParser();
                }

                return instance;
            }
        }

        internal static MobeelizerApplicationDefinition Parse(XDocument document)
        {
            return Instance.ParseDefinition(document);
        }

        private MobeelizerApplicationDefinition ParseDefinition(XDocument document)
        {  
            MobeelizerApplicationDefinition definition = new MobeelizerApplicationDefinition();
            XElement application = document.Root;
            definition.Application = application.Attribute(MobeelizerDefinitionTag.APPLICATION_TAG).Value;
            definition.ConflictMode = application.Attribute(MobeelizerDefinitionTag.CONFLICT_MODE_TAG).Value;
            definition.Vendor = application.Attribute(MobeelizerDefinitionTag.VENDOR_TAG).Value;
            XElement devices = application.Element(XName.Get(MobeelizerDefinitionTag.DEVICES_TAG, MobeelizerDefinitionTag.NAMESPACE));
            if (devices != null)
            {
                definition.Devices = new List<MobeelizerDeviceDefinition>();
                foreach (XElement device in devices.Elements(XName.Get(MobeelizerDefinitionTag.DEVICE_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    MobeelizerDeviceDefinition deviceDef = new MobeelizerDeviceDefinition();
                    deviceDef.Name = device.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                    definition.Devices.Add(deviceDef);
                }
            }

            XElement groups = application.Element(XName.Get(MobeelizerDefinitionTag.GROUPS_TAG, MobeelizerDefinitionTag.NAMESPACE));
            if (groups != null)
            {
                definition.Groups = new List<MobeelizerGroupDefinition>();
                foreach (XElement group in groups.Elements(XName.Get(MobeelizerDefinitionTag.GROUP_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    MobeelizerGroupDefinition groupDef = new MobeelizerGroupDefinition();
                    groupDef.Name = group.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                    definition.Groups.Add(groupDef);
                }
            }

            XElement roles = application.Element(XName.Get(MobeelizerDefinitionTag.ROLES_TAG, MobeelizerDefinitionTag.NAMESPACE));
            if (roles != null)
            {
                definition.Roles = new List<MobeelizerRoleDefinition>();
                foreach (XElement role in roles.Elements(XName.Get(MobeelizerDefinitionTag.ROLE_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    MobeelizerRoleDefinition roleDef = new MobeelizerRoleDefinition();
                    roleDef.Device = role.Attribute(MobeelizerDefinitionTag.DEVICE_TAG).Value;
                    roleDef.Group = role.Attribute(MobeelizerDefinitionTag.GROUP_TAG).Value;
                    definition.Roles.Add(roleDef);
                }
            }

            XElement models = application.Element(XName.Get(MobeelizerDefinitionTag.MODELS_TAG, MobeelizerDefinitionTag.NAMESPACE));
            if (models != null)
            {
                definition.Models = new List<MobeelizerModelDefinition>();
                foreach (XElement model in models.Elements(XName.Get(MobeelizerDefinitionTag.MODEL_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    definition.Models.Add(ParseModel(model));
                }
            }

            return definition;
        }

        private MobeelizerModelDefinition ParseModel(XElement model)
        {
            MobeelizerModelDefinition modelDef = new MobeelizerModelDefinition();
            modelDef.Name = model.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
            XElement fields = model.Element(XName.Get(MobeelizerDefinitionTag.FIELDS_TAG, MobeelizerDefinitionTag.NAMESPACE));
            if (fields != null)
            {
                modelDef.Fields = new List<MobeelizerModelFieldDefinition>();
                foreach (XElement field in fields.Elements(XName.Get(MobeelizerDefinitionTag.FIELD_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    modelDef.Fields.Add(ParseField(field));
                }
            }

            XElement credentials = model.Element(XName.Get(MobeelizerDefinitionTag.CREDENTIALS_TAG, MobeelizerDefinitionTag.NAMESPACE));
            modelDef.Credentials = new List<MobeelizerModelCredentialsDefinition>();
            foreach (XElement credential in credentials.Elements(XName.Get(MobeelizerDefinitionTag.CREDENTIAL_TAG, MobeelizerDefinitionTag.NAMESPACE)))
            {
                modelDef.Credentials.Add(ParseModelCredentials(credential));
            }

            return modelDef;
        }

        private MobeelizerModelCredentialsDefinition ParseModelCredentials(XElement credential)
        {
            MobeelizerModelCredentialsDefinition credentialDef = new MobeelizerModelCredentialsDefinition();
            credentialDef.CreateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.CREATE_ALLOWED_TAG).Value, true);
            credentialDef.ReadAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.READ_ALLOWED_TAG).Value, true);
            credentialDef.UpdateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.UPDATE_ALLOWED_TAG).Value, true);
            credentialDef.Role = credential.Attribute(MobeelizerDefinitionTag.ROLE_TAG).Value;
            credentialDef.IsResolveConflictAllowed = Boolean.Parse(credential.Attribute(MobeelizerDefinitionTag.RESOLVE_CONFLICT_ALLOWED_TAG).Value);
            credentialDef.DeleteAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.DELETE_ALLOWED_TAG).Value, true);
            return credentialDef;
        }

        private MobeelizerModelFieldCredentialsDefinition ParseModelFieldCredentials(XElement credential)
        {
            MobeelizerModelFieldCredentialsDefinition credentialDef = new MobeelizerModelFieldCredentialsDefinition();
            credentialDef.CreateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.CREATE_ALLOWED_TAG).Value, true);
            credentialDef.ReadAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.READ_ALLOWED_TAG).Value, true);
            credentialDef.UpdateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.UPDATE_ALLOWED_TAG).Value, true);
            credentialDef.Role = credential.Attribute(MobeelizerDefinitionTag.ROLE_TAG).Value;
            return credentialDef;                
        }

        private MobeelizerModelFieldDefinition ParseField(XElement field)
        {
            MobeelizerModelFieldDefinition fieldDef = new MobeelizerModelFieldDefinition();
            fieldDef.Name = field.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
            fieldDef.IsRequired = Boolean.Parse(field.Attribute(MobeelizerDefinitionTag.REQUIRED_TAG).Value);
            fieldDef.Type = (MobeelizerFieldType)Enum.Parse(typeof(MobeelizerFieldType), field.Attribute(MobeelizerDefinitionTag.TYPE_TAG).Value, true);
            if(field.Attribute(MobeelizerDefinitionTag.DEFAULT_VALUE_TAG)!= null)
            {
                fieldDef.DefaultValue = field.Attribute(MobeelizerDefinitionTag.DEFAULT_VALUE_TAG).Value;
            }

            XElement fieldCredentials = field.Element(XName.Get(MobeelizerDefinitionTag.CREDENTIALS_TAG, MobeelizerDefinitionTag.NAMESPACE));
            if (fieldCredentials != null)
            {
                fieldDef.Credentials = new List<MobeelizerModelFieldCredentialsDefinition>();
                foreach (XElement credential in fieldCredentials.Elements(XName.Get(MobeelizerDefinitionTag.CREDENTIAL_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    fieldDef.Credentials.Add(ParseModelFieldCredentials(credential));
                }
            }

            XElement options = field.Element(XName.Get(MobeelizerDefinitionTag.OPTIONS_TAG, MobeelizerDefinitionTag.NAMESPACE));
            fieldDef.Options = new Dictionary<string, string>();
            if (options != null)
            {
                foreach (XElement option in options.Elements(XName.Get(MobeelizerDefinitionTag.OPTION_TAG, MobeelizerDefinitionTag.NAMESPACE)))
                {
                    String name = option.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                    String value = option.Value;
                    fieldDef.Options.Add(name, value);
                }
            }
            return fieldDef;
        }
    }
}
