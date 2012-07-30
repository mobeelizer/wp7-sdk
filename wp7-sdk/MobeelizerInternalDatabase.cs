using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7
{
    internal class MobeelizerInternalDatabase
    {
        public MobeelizerInternalDatabase()
        {
            using (var db = new MobeelizerInternalDatabaseContext())
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();
                }
            }
        }

        public bool IsInitialSyncRequired(String instance, String instanceGuid, String user)
        {
            bool retVal = true;
            using (var db = new MobeelizerInternalDatabaseContext())
            {
                try
                {
                    var query = from role in db.Roles where role.Instance == instance && role.User == user select new { IsInitialSyncRequired = role.InitialSyncRequired, InstanceGuid = role.InstanceGuid };
                    var reponse = query.Single();
                    if (reponse.InstanceGuid == instanceGuid && !reponse.IsInitialSyncRequired)
                    {
                        retVal = false;
                    }
                }
                catch (InvalidOperationException)
                { }
            }

            return retVal;
        }

        public void SetInitialSyncAsNotRequired(String instance, String user)
        {
            using (var db = new MobeelizerInternalDatabaseContext())
            {
                try
                {
                    var query = from r in db.Roles where r.Instance == instance && r.User == user select r;
                    MobeelizerRoleEntity roleEntity = query.Single();
                    roleEntity.InitialSyncRequired = false;
                    db.SubmitChanges();
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public void SetRoleAndInstanceGuid(String instance, String user, String password, String role, String instanceGuid)
        {
            MobeelizerRoleEntity roleEntity = null;
            using (var db = new MobeelizerInternalDatabaseContext())
            {
                try
                {
                    var query = from r in db.Roles where r.Instance == instance && r.User == user select r;
                    roleEntity = query.Single();
                }
                catch (InvalidOperationException)
                {
                }

                if (roleEntity == null)
                {
                    roleEntity = new MobeelizerRoleEntity()
                    {
                        Instance = instance,
                        User = user,
                        Password = this.GetMd5(password),
                        Role = role,
                        InstanceGuid = instanceGuid,
                        InitialSyncRequired = true
                    };
                    db.Roles.InsertOnSubmit(roleEntity);
                }
                else
                {
                    roleEntity.Password = this.GetMd5(password);
                    roleEntity.Role = role;
                    roleEntity.InstanceGuid = instanceGuid;
                }

                db.SubmitChanges();
            }
        }

        public void ClearRoleAndInstanceGuid(String instance, String user)
        {
            using (var db = new MobeelizerInternalDatabaseContext())
            {
                try
                { 
                    var query = from r in db.Roles where r.Instance == instance && r.User == user select r;
                    MobeelizerRoleEntity roleEntity = query.Single();
                    roleEntity.Role = null;
                    roleEntity.InstanceGuid = null;
                    db.SubmitChanges();
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        public String[] GetRoleAndInstanceGuid(String instance, String user, String password)
        {
            String role = null;
            String instanceGuid = null;
            using (var db = new MobeelizerInternalDatabaseContext())
            {
                try
                {
                    var query = from r in db.Roles where r.Instance == instance && r.User == user && r.Password == GetMd5(password) select r;
                    MobeelizerRoleEntity roleEntity = query.Single();
                    role = roleEntity.Role;
                    instanceGuid = roleEntity.InstanceGuid;
                }
                catch (InvalidOperationException)
                {
                }
            }

            return new String[] { role, instanceGuid };
        }

        private String GetMd5(String password)
        {

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(password);
            HashAlgorithm hash = new SHA1Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            String hashValue = Convert.ToBase64String(hashBytes);
            return hashValue;
        }
    }
}
