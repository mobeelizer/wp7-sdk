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
using System.Xml.Linq;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerDefinitionParser
    {
        
        internal static MobeelizerApplicationDefinition Parse(XDocument document)
        {
            MobeelizerApplicationDefinition definition = new MobeelizerApplicationDefinition();
            XElement application = document.Root;
            definition.Application = application.Attribute(MobeelizerDefinitionTag.APPLICATION_TAG).Value;
            definition.ConflictMode = application.Attribute(MobeelizerDefinitionTag.CONFLICT_MODE_TAG).Value;
            definition.Vendor = application.Attribute(MobeelizerDefinitionTag.VENDOR_TAG).Value;
            XElement devices = application.Element(MobeelizerDefinitionTag.DEVICES_TAG);
            definition.Devices = new List<MobeelizerDeviceDefinition>();
            foreach (XElement device in devices.Elements(MobeelizerDefinitionTag.DEVICE_TAG))
            {
                MobeelizerDeviceDefinition deviceDef = new MobeelizerDeviceDefinition();
                deviceDef.Name = device.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                definition.Devices.Add(deviceDef);
            }

            XElement groups = application.Element(MobeelizerDefinitionTag.GROUPS_TAG);
            definition.Groups = new List<MobeelizerGroupDefinition>();
            foreach (XElement group in groups.Elements(MobeelizerDefinitionTag.GROUP_TAG))
            {
                MobeelizerGroupDefinition groupDef = new MobeelizerGroupDefinition();
                groupDef.Name = group.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                definition.Groups.Add(groupDef);
            }

            XElement roles = application.Element(MobeelizerDefinitionTag.ROLES_TAG);
            definition.Roles = new List<MobeelizerRoleDefinition>();
            foreach (XElement role in roles.Elements(MobeelizerDefinitionTag.ROLE_TAG))
            {
                MobeelizerRoleDefinition roleDef = new MobeelizerRoleDefinition();
                roleDef.Device = role.Attribute(MobeelizerDefinitionTag.DEVICE_TAG).Value;
                roleDef.Group = role.Attribute(MobeelizerDefinitionTag.GROUP_TAG).Value;
                definition.Roles.Add(roleDef);
            }

            XElement models = application.Element(MobeelizerDefinitionTag.MODELS_TAG);
            definition.Models = new List<MobeelizerModelDefinition>();
            foreach (XElement model in models.Elements(MobeelizerDefinitionTag.MODEL_TAG))
            {
                MobeelizerModelDefinition modelDef = new MobeelizerModelDefinition();
                modelDef.Name = model.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                modelDef.Fields = new List<MobeelizerModelFieldDefinition>();
                XElement fields = model.Element(MobeelizerDefinitionTag.FIELDS_TAG);
                foreach (XElement field in fields.Elements(MobeelizerDefinitionTag.FIELD_TAG))
                {
                    MobeelizerModelFieldDefinition fieldDef = new MobeelizerModelFieldDefinition();
                    fieldDef.Name = field.Attribute(MobeelizerDefinitionTag.NAME_TAG).Value;
                    fieldDef.IsRequired = Boolean.Parse(field.Attribute(MobeelizerDefinitionTag.REQUIRED_TAG).Value);
                    fieldDef.Type = (MobeelizerFieldType)Enum.Parse(typeof(MobeelizerFieldType), field.Attribute(MobeelizerDefinitionTag.TYPE_TAG).Value, true);
                    XElement fieldCredentials = field.Element(MobeelizerDefinitionTag.CREDENTIALS_TAG);
                    fieldDef.Credentials = new List<MobeelizerModelFieldCredentialsDefinition>();
                    foreach (XElement credential in fieldCredentials.Elements(MobeelizerDefinitionTag.CREDENTIAL_TAG))
                    {
                        MobeelizerModelFieldCredentialsDefinition credentialDef = new MobeelizerModelFieldCredentialsDefinition();
                        credentialDef.CreateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.CREATE_ALLOWED_TAG).Value, true);
                        credentialDef.ReadAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.READ_ALLOWED_TAG).Value, true);
                        credentialDef.UpdateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.UPDATE_ALLOWED_TAG).Value, true);
                        credentialDef.Role = credential.Attribute(MobeelizerDefinitionTag.ROLE_TAG).Value;
                        fieldDef.Credentials.Add(credentialDef);
                    }
                    modelDef.Fields.Add(fieldDef);
                }

                XElement credentials = model.Element(MobeelizerDefinitionTag.CREDENTIALS_TAG);
                modelDef.Credentials = new List<MobeelizerModelCredentialsDefinition>();
                foreach (XElement credential in credentials.Elements(MobeelizerDefinitionTag.CREDENTIAL_TAG))
                {
                    MobeelizerModelCredentialsDefinition credentialDef = new MobeelizerModelCredentialsDefinition();
                    credentialDef.CreateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.CREATE_ALLOWED_TAG).Value, true);
                    credentialDef.ReadAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.READ_ALLOWED_TAG).Value, true);
                    credentialDef.UpdateAllowed = (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.UPDATE_ALLOWED_TAG).Value, true);
                    credentialDef.Role = credential.Attribute(MobeelizerDefinitionTag.ROLE_TAG).Value;
                    credentialDef.IsResolveConflictAllowed = Boolean.Parse(credential.Attribute(MobeelizerDefinitionTag.RESOLVE_CONFLICT_ALLOWED_TAG).Value);
                    credentialDef.DeleteAllowed= (MobeelizerCredential)Enum.Parse(typeof(MobeelizerCredential), credential.Attribute(MobeelizerDefinitionTag.DELETE_ALLOWED_TAG).Value, true);
                    modelDef.Credentials.Add(credentialDef);
                    // TODO : Test it
                }
            }

            return definition;
        }
    }
}
