using System;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    public class MobeelizerGroupDefinition : IMobeelizerDefinition
    {
        public String Name { get; set; }

        public String DigestString
        {
            get
            {
                return this.Name;
            }
        }
    }
}
