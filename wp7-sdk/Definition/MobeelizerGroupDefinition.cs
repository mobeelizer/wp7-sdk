using System;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    class MobeelizerGroupDefinition : IMobeelizerDefinition
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
