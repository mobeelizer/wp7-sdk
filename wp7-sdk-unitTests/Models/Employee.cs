using System;
using System.Data.Linq.Mapping;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_sdk_unitTests.Models
{
    [Table]
    public class Employee : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey = true)]
        public override String guid { get; set; }

        [Column()]
        public String department { get; set; }

        [Column()]
        public String name { get; set; }

        [Column()]
        public String position { get; set; }

        [Column()]
        public double salary { get; set; }

        [Column()]
        public String surname { get; set; }
    }
}
