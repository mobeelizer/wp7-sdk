using Microsoft.Practices.Mobile.Configuration;

namespace Com.Mobeelizer.Mobile.Wp7.Configuration
{
    public class MobeelizerConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Get the connection Items.
        /// </summary>
        [ConfigurationProperty("properties")]
        internal MobeelizerPropertiesCollection AppSettings
        {
            get
            {
                return (MobeelizerPropertiesCollection)(this["properties"]);
            }
        }
    }
}
