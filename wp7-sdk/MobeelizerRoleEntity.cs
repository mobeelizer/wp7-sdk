using System;
using System.Data.Linq.Mapping;

namespace Com.Mobeelizer.Mobile.Wp7
{
    [Table]
    internal class MobeelizerRoleEntity
    {
        [Column( Name = "Id", DbType = "BigInt IDENTITY NOT NULL", IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
        public int Id { get; set; }

        [Column(CanBeNull = false)]
        public String Instance { get; set; }

        [Column(CanBeNull = false)]
        public String User { get; set; }

        [Column(CanBeNull = false)]
        public String Password { get; set; }

        [Column()]
        public String Role { get; set; }

        [Column()]
        public String InstanceGuid { get; set; }

        [Column(CanBeNull = false)]
        public bool InitialSyncRequired { get; set; }
    }
}
