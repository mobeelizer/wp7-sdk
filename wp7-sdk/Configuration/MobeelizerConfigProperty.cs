using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7.Configuration
{
    internal class MobeelizerConfigProperty : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        internal string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        internal string Value
        {
            get
            {
                return (string)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }
    }
}
