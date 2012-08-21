using System;
using System.Data.Linq.Mapping;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    [Table]
    internal class MobeelizerModelMetadata
    {
        [Column(IsPrimaryKey = true)]
        public String Guid { get; set; }

        [Column(CanBeNull= false)]
        public String Model { get; set; }

        [Column()]
        public bool ModificationLock { get; set; }
    }
}
