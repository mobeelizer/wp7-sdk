using System;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    class MobeelizerRoleDefinition : IMobeelizerDefinition
    {
        internal String Group { get; set; }

        internal String Device { get; set; }

        public String DigestString
        {
            get
            {
                return String.Format("{{0}${1}}", this.Group, this.Device);
            }
        }

        internal Object ResolveName()
        {
            return this.Group + "-" + this.Device;
        }
    }
}
