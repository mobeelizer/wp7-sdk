using System;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    class MobeelizerDeviceDefinition : IMobeelizerDefinition
    {
        internal String Name { get; set; }

        public String DigestString
        {
            get
            {
                return this.Name;
            }
        }
    }
}
