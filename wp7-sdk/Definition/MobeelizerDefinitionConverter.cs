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
using Com.Mobeelizer.Mobile.Wp7.Api;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7.Model;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    internal class MobeelizerDefinitionConverter
    {
        // TODO: sprawdzanie czy modele sa zgodne z definicja

        internal IList<MobeelizerModel> Convert(MobeelizerApplicationDefinition definition, String entityPackage, String role)
        {
            CheckRole(definition, role);

            IList<MobeelizerModel> models = new List<MobeelizerModel>();

            foreach (MobeelizerModelDefinition radModel in definition.Models)
            {
                MobeelizerModelCredentialsDefinition modelCredentials = HasAccess(radModel, role);
                if (modelCredentials == null)
                {
                    continue;
                }

                Type type = null;
                if (entityPackage != null)
                {
                    type = FindType(radModel, entityPackage);
                }

                IList<MobeelizerField> fields = new List<MobeelizerField>();
                foreach (MobeelizerModelFieldDefinition radField in radModel.Fields)
                {
                    MobeelizerModelFieldCredentialsDefinition fieldCredentials = HasAccess(radField, role);

                    if (fieldCredentials == null)
                    {
                        continue;
                    }

                    fields.Add(new MobeelizerField(type, radField, fieldCredentials));
                }

                MobeelizerModel model = new MobeelizerModel(type, radModel.Name, modelCredentials, fields);
                models.Add(model);
            }

            return models;
        }

        private MobeelizerModelFieldCredentialsDefinition HasAccess(MobeelizerModelFieldDefinition field, string role)
        {
            foreach (MobeelizerModelFieldCredentialsDefinition credentials in field.Credentials)
            {
                if (credentials.Role.Equals(role))
                {
                    if (credentials.CreateAllowed != MobeelizerCredential.NONE
                            || credentials.UpdateAllowed != MobeelizerCredential.NONE
                            || credentials.ReadAllowed != MobeelizerCredential.NONE)
                    {
                        return credentials;
                    }
                    break;
                }
            }

            return null;
        }

        private Type FindType(MobeelizerModelDefinition radModel, string entityPackage)
        {
            String typeFullName = entityPackage.Replace(",", String.Format(".{0},", radModel.Name));
            Type type = null;
            try
            {
                type = Type.GetType(typeFullName);
            }
            catch (ArgumentException e)
            {
                throw new InvalidOperationException(e.Message, e);
            }

            return type;
        }

        private MobeelizerModelCredentialsDefinition HasAccess(MobeelizerModelDefinition model, string role)
        {
            foreach (MobeelizerModelCredentialsDefinition credentials in model.Credentials)
            {
                if (credentials.Role.Equals(role))
                {
                    if (credentials.CreateAllowed != MobeelizerCredential.NONE
                            || credentials.UpdateAllowed != MobeelizerCredential.NONE
                            || credentials.ReadAllowed != MobeelizerCredential.NONE
                            || credentials.DeleteAllowed != MobeelizerCredential.NONE)
                    {
                        return credentials;
                    }

                    break;
                }
            }

            return null;
        }

        private void CheckRole(MobeelizerApplicationDefinition definition, String role)
        {
            foreach (MobeelizerRoleDefinition radRole in definition.Roles)
            {
                if (radRole.ResolveName().Equals(role))
                {
                    return;
                }
            }
            throw new InvalidOperationException("Role " + role + " doesn't exist in definition.");
        }
    }
}
