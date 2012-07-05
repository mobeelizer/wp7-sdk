using System.Data.Linq;

namespace Com.Mobeelizer.Mobile.Wp7 
{
    internal class MobeelizerInternalDatabaseContext : DataContext
    {
        public MobeelizerInternalDatabaseContext()
            : base("DataSource=isostore:/internal.sdf")
        {
        }

        internal Table<MobeelizerRoleEntity> Roles
        {
            get
            {
                return this.GetTable<MobeelizerRoleEntity>(); 
            }
        }
    }
}
