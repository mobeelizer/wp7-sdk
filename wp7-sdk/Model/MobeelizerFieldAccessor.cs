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

namespace Com.Mobeelizer.Mobile.Wp7.Model
{
    public class MobeelizerFieldAccessor
    {
        public MobeelizerFieldAccessor(Type type, String name)
        {
            this.Type = type;
            this.Name = name;
        }

        public Type Type { get; private set; }

        public string Name { get; private set; }
    }
}
