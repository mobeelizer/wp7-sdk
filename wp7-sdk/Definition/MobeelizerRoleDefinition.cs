using System;
using System.Text;

namespace Com.Mobeelizer.Mobile.Wp7.Definition
{
    internal class MobeelizerRoleDefinition : IMobeelizerDefinition
    {
        public String Group { get; set; }

        public String Device { get; set; }

        public String DigestString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{").Append(this.Group);
                sb.Append("$");
                sb.Append(this.Device).Append("}");
                return sb.ToString();
            }
        }

        public Object ResolveName()
        {
            return String.Format("{0}-{1}", this.Group, this.Device);
        }
    }
}
