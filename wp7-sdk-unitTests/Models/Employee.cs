﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;

namespace wp7_sdk_unitTests.Models
{
    [Table]
    public class Employee
    {
        [Column(IsPrimaryKey = true)]
        public String guid { get; set; }

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
