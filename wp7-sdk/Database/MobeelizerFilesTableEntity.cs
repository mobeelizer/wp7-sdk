using System;
using System.Data.Linq.Mapping;

namespace Com.Mobeelizer.Mobile.Wp7.Database
{
    [Table]
    internal class MobeelizerFilesTableEntity
    {
        [Column(IsPrimaryKey= true)]
        public String Guid { get; set; }

        [Column(CanBeNull=false)]
        public String Path { get; set; }

        [Column(CanBeNull=false)]
        public int Modyfied { get; set; }
    }
}
