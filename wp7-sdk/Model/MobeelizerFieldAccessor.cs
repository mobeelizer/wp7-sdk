using System;

namespace Com.Mobeelizer.Mobile.Wp7.Model
{
    internal class MobeelizerFieldAccessor
    {
        internal MobeelizerFieldAccessor(Type type, String name)
        {
            this.Type = type;
            this.Name = name;
        }

        internal Type Type { get; private set; }

        internal string Name { get; private set; }
    }
}
