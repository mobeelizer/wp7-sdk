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
using System.Data.Linq.Mapping;

namespace wp7_sdk_unitTests.Models.test2Production
{
    [Table]
    public class Blog
    {
        [Column(IsPrimaryKey = true)]
        public String guid { get; set; }

        [Column()]
        public String content { get; set; }

        [Column()]
        public String title { get; set; }
    }
}
