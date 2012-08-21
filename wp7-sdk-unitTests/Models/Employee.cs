using System;
using System.Data.Linq.Mapping;
using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_sdk_unitTests.Models
{
    [Table]
    public class Employee : MobeelizerWp7Model
    {
        [Column(IsPrimaryKey = true)]
        public override String Guid { get; set; }

        [Column()]
        public override String Owner { get; set; }

        [Column()]
        public override bool Conflicted { get; set; }

        [Column()]
        public override bool Deleted { get; set; }

        [Column()]
        public override bool Modified { get; set; }

        [Column()]
        public String Department { get; set; }

        [Column()]
        public String Name { get; set; }

        [Column()]
        public String Position { get; set; }

        [Column()]
        public double Salary { get; set; }

        [Column()]
        public String Surname { get; set; }
    }
}
